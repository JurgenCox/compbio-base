using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Drmaa {
    public class DrmaaExecutor{
        private class Job{
            readonly string JobId;
            readonly JobTemplate JobTemplate;
            Job(string jobId, JobTemplate jobTemplate){
                JobId = jobId;
                JobTemplate = jobTemplate;
            }
        }

        private readonly int nConcurrentTasks;
        private readonly bool abortOnFirstError;

        public DrmaaExecutor(int nConcurrentTasks, bool abortOnFirstError=false){
            this.nConcurrentTasks = nConcurrentTasks;
            this.abortOnFirstError = abortOnFirstError;
        }

        public void Execute(IEnumerable<JobTemplate> jobTemplates){
            var q = new Queue<JobTemplate>(jobTemplates);

            var activeJobs = new Dictionary<string, JobTemplate>();
            while(true){
                foreach(var entry in activeJobs.ToList()){
                    var status = Session.JobStatus(entry.Key);
                    switch(status){
                        case Status.Done: 
                            Console.WriteLine($"Job {entry.Key} done");
                            activeJobs.Remove(entry.Key);
                            break;

                        case Status.Failed:
                            Console.WriteLine($"Job {entry.Key} failed");
                            activeJobs.Remove(entry.Key);
                            break;
                    }
                }
                Thread.Sleep(1000);

                var toAdd = Math.Max(0, nConcurrentTasks - activeJobs.Count);
                toAdd = Math.Min(q.Count, toAdd);
                for(var i = 0; i < toAdd; i++){
                    var jt = q.Dequeue();
                    var jobId = jt.Submit();
                    activeJobs.Add(jobId, jt);
                    Console.WriteLine($"Submitted job {jt.JobName} with id {jobId}");
                }

                Console.WriteLine($"Active jobs: {activeJobs.Count}, Queued jobs: {q.Count}");
            }
        }
    }
}