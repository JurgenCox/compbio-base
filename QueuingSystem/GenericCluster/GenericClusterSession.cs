using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace QueuingSystem.GenericCluster
{
    public class GenericClusterSession: ISession
    {
//        private const string SgeStatusCommand = "qstat -j {jobId}";
        private const string SgeSubmitCommand = "qsub ";
        private const int SleepIntervalMillis = 1000;
        
//        private readonly string statusCommand;
        private readonly string submitCommand;
        private static int _nextId = 1;
        private static Array _lock = new double[0];
        
        private readonly IDictionary<string, GenericClusterJobTemplate> _submittedJobs = new ConcurrentDictionary<string, GenericClusterJobTemplate>();
        public GenericClusterSession(
            string submitCommand)
        {
            this.submitCommand = submitCommand ?? SgeSubmitCommand;
//            this.statusCommand = statusCommand;
        }

        private string GetNextId()
        {
            // TODO: use Interlocked?
            lock (_lock)
            {
                var res = _nextId.ToString();
                _nextId += 1;
                return res;
            }
        }
        
        public Status JobStatus(string jobId)
        {
            // noop
            Console.Error.WriteLine($"Job status is not implemented for GenericClusters, jobId: {jobId}");
            return Status.Unknown;
        }

        public void JobControl(string jobId, Action action)
        {
            // noop
            Console.Error.WriteLine($"Job control is not implemented for GenericClusters, jobId: {jobId}, action: {action}");
        }

        public IJobTemplate AllocateJobTemplate()
        {
            return new GenericClusterJobTemplate(GetNextId(), this);
        }

        public Status WaitForJobBlocking(string jobId)
        {
            if (!_submittedJobs.TryGetValue(jobId, out var jt))
            {
                throw new QueuingSystemException(0, $"Job with id {jobId} not found");
            }

            Console.WriteLine($"WaitForJobBlocking({jobId})");
            while (!jt.isFinished)
            {
                Thread.Sleep(SleepIntervalMillis);
            }

            try
            {
                if (jt.isError)
                {
                    Console.WriteLine($"WaitForJobBlocking({jobId}): error");
                    return Status.Failed;
                }

                if (jt.isFinished)
                {
                    Console.WriteLine($"WaitForJobBlocking({jobId}): finished");
                    return Status.Success;
                }

                Console.WriteLine($"WaitForJobBlocking({jobId}): unknown status");
                return Status.Unknown;
            }
            finally
            {
                _submittedJobs.Remove(jobId);
                jt.Cleanup();
            }
        }

        public void Exit(string contact = null)
        {
            foreach (var kv in _submittedJobs)
            {
                var jobId = kv.Key;
                var jt = kv.Value;
                JobControl(jobId, Action.Terminate);
                jt.Cleanup();
            }
            _submittedJobs.Clear();
        }

        private string FormatTemplateString(string template, IDictionary<string, object> context)
        {
            foreach (var kv in context)
            {
                var key = kv.Key;
                var substitution = kv.Value;
                template = template.Replace($"{{{key}}}", (substitution ?? "").ToString());
            }

            return template;
        }
        
        private string FormatCommand(GenericClusterJobTemplate jobTemplate)
        {
            var context = new Dictionary<string, object>()
            {
                {"nativeSpec", jobTemplate.NativeSpecification},
                {"workDir", jobTemplate.WorkingDirectory},
                {"output", jobTemplate.OutputPath},
                {"input", jobTemplate.InputPath},
                {"error", jobTemplate.ErrorPath},
                {"id", jobTemplate.InternalId},
            };
            
            return FormatTemplateString(submitCommand, context);
        }
        internal string SubmitInternal(GenericClusterJobTemplate jobTemplate)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            var command = FormatCommand(jobTemplate);
            var args = Util.SplitCommandLine(command).ToList();
            var argsConcatenated = string.Join(" ", args.Skip(1).Select(a => $"\"{a}\""));
            startInfo.FileName = args[0];
            startInfo.Arguments = argsConcatenated + $" \"{jobTemplate.JobScriptPath}\"";
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            
            process.StartInfo = startInfo;

            Console.WriteLine($"Executing cmd: {startInfo.FileName} with arguments: {startInfo.Arguments}");
            process.Start();
            process.WaitForExit();

            var exitCode = process.ExitCode;
            if (exitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                var output = process.StandardOutput.ReadToEnd();
                var message = $"Job submit error, output: {output}, error: {error}";
                throw new GenericClusterException(exitCode, message);
            }
            
            _submittedJobs[jobTemplate.InternalId] = jobTemplate;

            Console.WriteLine($"Submitted Job: {jobTemplate.InternalId}");
            return jobTemplate.InternalId;
        }
    }
}