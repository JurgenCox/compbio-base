using System;
using System.IO;
using QueuingSystem.Drmaa;

namespace QueuingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            var session = DrmaaSession.GetInstance();
            
            session.Init();
            var n = 10;
            var workDir = Directory.GetCurrentDirectory();
            var jobName = $"test_job_{n}";
            var jt = session.AllocateJobTemplate();
            jt.JobName = jobName;
            jt.RemoteCommand = "sleep";
            jt.Arguments = new string[]{"10", };
            jt.WorkingDirectory = workDir;
            jt.OutputPath = ":"+Path.Combine(workDir, $"{jobName}.out");
            jt.ErrorPath = ":"+Path.Combine(workDir, $"{jobName}.err");

            var jobId = jt.Submit();
            var res = session.WaitForJobBlocking(jobId);
            Console.WriteLine(res);
        }
    }
}