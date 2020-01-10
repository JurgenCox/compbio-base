using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace QueueingSystem.GenericCluster
{
    public class GenericClusterJobTemplate: IJobTemplate
    {
        private readonly string jobScriptPath;
        private readonly string jobSuccessStatusPath;
        private readonly string jobErrorStatusPath;
        private readonly string internalId;
        private readonly string jobTemplateStr;

        public GenericClusterJobTemplate(string internalId, string jobTemplateStr)
        {
            this.internalId = internalId;
            this.jobTemplateStr = jobTemplateStr;
            var tempDir = ".";
            var randomName = Path.GetRandomFileName();
            jobScriptPath = Path.Combine(tempDir, $"{internalId}.{randomName}.jobscript");
            jobSuccessStatusPath = Path.Combine(tempDir, $"{internalId}.{randomName}.jobsuccess");
            jobErrorStatusPath = Path.Combine(tempDir, $"{internalId}.{randomName}.joberrord");
        }

        public bool isSuccess => File.Exists(jobSuccessStatusPath);

        public bool isError => File.Exists(jobErrorStatusPath);

        public bool isFinished => isSuccess || isError;
        
        public string InternalId => internalId;

        public string JobScriptPath => jobScriptPath;

        public string ReadStderr()
        {
            if (!File.Exists(ErrorPath))
            {
                return null;
            }

            return File.ReadAllText(ErrorPath);
        }

        public string ReadStdout()
        {
            if (!File.Exists(OutputPath))
            {
                return null;
            }

            return File.ReadAllText(OutputPath);
        }

        public void Cleanup()
        {
//            if (File.Exists(OutputPath))
//            {
//                File.Delete(OutputPath);
//            }
//            
//            if (File.Exists(ErrorPath))
//            {
//                File.Delete(ErrorPath);
//            }
//            
//            if (File.Exists(InputPath))
//            {
//                File.Delete(InputPath);
//            }
//            
//            if (File.Exists(jobScriptPath))
//            {
//                File.Delete(jobScriptPath);
//            }
        }

        public Dictionary<string, string> JobEnvironment { get; set; }
        public string InputPath { get; set; }
        public string OutputPath { get; set; }
        public string ErrorPath { get; set; }
        public bool JoinFiles { get; set; }
        public string[] Arguments { get; set; }
        public string WorkingDirectory { get; set; } = ".";
        
        public int Threads { get; set; }
        public long MaxMemorySize { get; set; }
        public string JobName { get; set; }

        public void WriteJobScript()
        {
            var command = string.Join(" ", Arguments.Select(a => $"\"{a}\""));
            var jobScriptStr = Util.FormatTemplateString(jobTemplateStr, new Dictionary<string, object>
            {
                {"command", command},
                {"success", jobSuccessStatusPath},
                {"error", jobErrorStatusPath}
            });
            
            Console.WriteLine($"Writing job script: \n{jobScriptStr}");
            File.WriteAllText(jobScriptPath, jobScriptStr);
        }
    }
}