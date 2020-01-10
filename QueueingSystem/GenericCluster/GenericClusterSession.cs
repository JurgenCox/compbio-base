using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace QueueingSystem.GenericCluster
{
    public class GenericClusterSession: ISession
    {
//        private const string SgeStatusCommand = "qstat -j {jobId}";
        private const string SgeSubmitCommand = "qsub ";
        private const int SleepIntervalMillis = 1000;

        private const string defaultJobTemplateStr =
            "#!/bin/bash\n" +
            "{command} && touch \"{success}\" || (touch \"{error}\"; exit 1);";

        private readonly SafeIdGenerator idGenerator = new SafeIdGenerator();
//        private readonly string statusCommand;
        private readonly string submitCommand;
        
        private readonly IDictionary<string, GenericClusterJobTemplate> _submittedJobs = new ConcurrentDictionary<string, GenericClusterJobTemplate>();
        public GenericClusterSession(
            string submitCommand,
            string jobTemplateStr = defaultJobTemplateStr)
        {
            this.submitCommand = submitCommand ?? SgeSubmitCommand;
//            this.statusCommand = statusCommand;
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
            return new GenericClusterJobTemplate(idGenerator.GetNextId(), defaultJobTemplateStr);
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

        public void Exit()
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

        private string FormatCommand(GenericClusterJobTemplate jobTemplate)
        {
            var context = new Dictionary<string, object>()
            {
                {"threads", jobTemplate.Threads},
                {"workDir", jobTemplate.WorkingDirectory},
                {"output", jobTemplate.OutputPath},
                {"input", jobTemplate.InputPath},
                {"error", jobTemplate.ErrorPath},
                {"id", jobTemplate.InternalId},
            };
            
            return Util.FormatTemplateString(submitCommand, context);
        }
        public string Submit(IJobTemplate jobTemplate)
        {
            var gJobTemplate = jobTemplate as GenericClusterJobTemplate;
            gJobTemplate.WriteJobScript();
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            var command = FormatCommand(gJobTemplate);
            var args = Util.SplitCommandLine(command).ToList();
            var argsConcatenated = string.Join(" ", args.Skip(1).Select(a => $"\"{a}\""));
            startInfo.FileName = args[0];
            startInfo.Arguments = argsConcatenated + $" \"{gJobTemplate.JobScriptPath}\"";
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
            
            _submittedJobs[gJobTemplate.InternalId] = gJobTemplate;

            Console.WriteLine($"Submitted Job: {gJobTemplate.InternalId}");
            return gJobTemplate.InternalId;
        }
    }
}