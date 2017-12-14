using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace DrmaaNet
{
    class Program
    {
        static void Main(string[] args)
        {
            Session.Init();
            var n = 10;
            var workDir = Directory.GetCurrentDirectory();
            var jobName = $"test_job_{n}";
            var jt = Session.AllocateJobTemplate();
            jt.JobName = jobName;
            jt.RemoteCommand = "sleep";
            jt.Arguments = new string[]{"10", };
            jt.WorkingDirectory = workDir;
            jt.OutputPath = ":"+Path.Combine(workDir, $"{jobName}.out");
            jt.ErrorPath = ":"+Path.Combine(workDir, $"{jobName}.err");

            var jobId = jt.Submit();
            var res = Session.WaitForJobBlocking(jobId);
            Console.WriteLine(res);
//            
//            var templates = Enumerable.Range(0, 10).Select(n =>
//            {
//                var workDir = Directory.GetCurrentDirectory();
//                var jobName = $"test_job_{n}";
//                var jt = Session.AllocateJobTemplate();
//                jt.JobName = jobName;
//                jt.RemoteCommand = "sleep";
//                jt.Arguments = new string[]{"10", };
//                jt.WorkingDirectory = workDir;
//                jt.OutputPath = ":"+Path.Combine(workDir, $"{jobName}.out");
//                jt.ErrorPath = ":"+Path.Combine(workDir, $"{jobName}.err");
////                jt.NativeSpecification = " -pe make 22";
//                return jt;
//            });

//            var executor = new DrmaaExecutor(3);
//            executor.Execute(templates);
        }
    }
}