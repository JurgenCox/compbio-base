using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using k8s;
using k8s.Models;
using QueuingSystem.GenericCluster;

namespace QueuingSystem.Kubernetes
{
    public class KubernetesSession: ISession
    {
        private readonly IKubernetes client;
        private readonly string _namespace;
        private readonly string containerId;
        private readonly HashSet<string> activeJobs = new HashSet<string>();
        private readonly SafeIdGenerator idGenerator = new SafeIdGenerator();
        
        public KubernetesSession(string @namespace, string containerId, KubernetesClientConfiguration config = null)
        {
            _namespace = @namespace;
            this.containerId = containerId;
            if (config == null)
            {
                config = new KubernetesClientConfiguration {  Host = "http://127.0.0.1:8001" };
            }
            client = new k8s.Kubernetes(config);
        }

        public Status JobStatus(string jobId)
        {
            var job = client.ReadNamespacedJobStatus(jobId, _namespace);
            return JobToStatus(job);
        }

        public void JobControl(string jobId, Action action)
        {
            // TODO: 
            Console.Error.WriteLine($"Job control is not implemented for GenericClusters, jobId: {jobId}, action: {action}");
        }

        public IJobTemplate AllocateJobTemplate()
        {
            return new GenericClusterJobTemplate(idGenerator.GetNextId());
        }

        public static Status JobToStatus(V1Job job)
        {
            if (job.Status.Failed == job.Spec.BackoffLimit)
            {
                return Status.Failed;
            }
            
            if (job.Status.Active != null)
            {
                return Status.Running;
            }

            if (job.Status.Succeeded != null)
            {
                return Status.Success;
            }

            return Status.Queued;
        }
        public Status WaitForJobBlocking(string jobId)
        {
            var watcher = client.WatchNamespacedJobAsync(jobId, _namespace).GetAwaiter().GetResult();
            var status = Status.Unknown;
            ManualResetEvent signal = new ManualResetEvent(false);
            watcher.OnClosed += () =>
            {
                status = Status.Failed;
                signal.Set();
            };

            watcher.OnError += exception =>
            {
                status = Status.Failed;
                signal.Set();
            };

            watcher.OnEvent += (type, job) =>
            {
                var s = JobToStatus(job);
                if (s != Status.Running)
                {
                    status = s;
                    signal.Set();
                }
            };
            signal.WaitOne();
            activeJobs.Remove(jobId);
            return status;

        }

        public void Exit(string contact = null)
        {
            // TODO: stop all active jobs
        }

        private V1Job CreateJob(IJobTemplate jt)
        {
            string jobName = jt.JobName ?? "";
            jobName = Regex.Replace(jobName.ToLower(), @"[^a-z\d]", "-");
            return new V1Job
            {
                Kind = "Job",
                ApiVersion = "batch/v1",
                Metadata = new V1ObjectMeta
                {
                    GenerateName = jobName 
                },
                Spec = new V1JobSpec
                {
                    BackoffLimit = 1,
                    Template = new V1PodTemplateSpec
                    {
                        Spec = new V1PodSpec
                        {
                            Containers = new []
                            {
                                new V1Container
                                {
                                    Name = $"{jobName}-container",
                                    Image = containerId,
                                    Command = jt.Arguments
                                }
                            },
                            RestartPolicy = "Never"
                        }
                    }
                }
            };
        }
        public string Submit(IJobTemplate jobTemplate)
        {
            var jobToSubmit = CreateJob(jobTemplate);
            var res = client.CreateNamespacedJob(jobToSubmit, _namespace);
            var jobId = res.Metadata.Name;
            activeJobs.Add(jobId);
            return jobId;
        }
    }
}