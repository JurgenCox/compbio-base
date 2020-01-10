using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using k8s;
using k8s.Models;
using QueueingSystem.GenericCluster;

namespace QueueingSystem.Kubernetes
{
    public class KubernetesSession: ISession
    {
        private readonly IKubernetes client;
        private readonly string _namespace;
        private readonly string containerId;
        private readonly ConcurrentBag<string> activeJobs = new ConcurrentBag<string>();
        private readonly SafeIdGenerator idGenerator = new SafeIdGenerator();
        private readonly IList<(string host, string mount)> _volumes;
        
        public KubernetesSession(string @namespace, string containerId, string volumesStr, string host)
        {
            _namespace = @namespace;
            this.containerId = containerId;
            KubernetesClientConfiguration config = new KubernetesClientConfiguration
            {
                Host = host
            };

            client = new k8s.Kubernetes(config);
            _volumes = ParseVolumes(volumesStr);
        }
        private static IList<(string host, string mount)> ParseVolumes(string volumesStr)
        {
            if (string.IsNullOrEmpty(volumesStr))
            {
                return new List<(string host, string mount)>();
            }
			
            var tokens = volumesStr.Split(',');
            return tokens.Select(t =>
            {
                var subt = t.Split(':');
                string host = subt[0];
                string mount = subt[1];
                return (host, mount);
            }).ToArray();
        }
        
        public Status JobStatus(string jobId)
        {
            var job = client.ReadNamespacedJobStatus(jobId, _namespace);
            return JobToStatus(job);
        }

        public void JobControl(string jobId, Action action)
        {
            switch (action)
            {
                case Action.Suspend:
                    Console.Error.WriteLine($"Job control is not implemented for GenericClusters, jobId: {jobId}, action: {action}");
                    break;
                case Action.Resume:
                    Console.Error.WriteLine($"Job control is not implemented for GenericClusters, jobId: {jobId}, action: {action}");
                    break;
                case Action.Terminate:
                    client.DeleteNamespacedJob(jobId, _namespace);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }

        public IJobTemplate AllocateJobTemplate()
        {
            // TODO: use own JobTemplate class
            return new GenericClusterJobTemplate(idGenerator.GetNextId(), null);
        }

        public static Status JobToStatus(V1Job job)
        {
            var status = job.Status;
            if (status.Active == null && status.Failed != null)
            {
                return Status.Failed;
            }
            
            if (status.Failed > job.Spec.BackoffLimit)
            {
                return Status.Failed;
            }
            
            if (status.Active != null)
            {
                return Status.Running;
            }

            if (status.Succeeded != null)
            {
                return Status.Success;
            }

            return Status.Queued;
        }
        public Status WaitForJobBlocking(string jobId)
        {
            Status status = Status.Unknown;
            string statusStrOld = "";
            while (true)
            {
                var job = client.ReadNamespacedJobStatus(jobId, _namespace);
                status = JobToStatus(job);
                var statusStr = Newtonsoft.Json.JsonConvert.SerializeObject(job.Status);

                if (statusStrOld != statusStr)
                {
                    Console.WriteLine($"ReadNamespacedJobStatus({jobId}) = Status: {statusStr}. Setting status to {status}");    
                }

                statusStrOld = statusStr;
                
                if (status != Status.Running && status != Status.Queued)
                {
                    break;
                }
                Thread.Sleep(1000);
            }

            return status;
        }

        public void Exit()
        {
            foreach (var jobId in activeJobs)
            {
                JobControl(jobId, Action.Terminate);
            }
        }

        private V1Job CreateJob(IJobTemplate jt)
        {
            string jobName = jt.JobName ?? "";
            jobName = Regex.Replace(jobName.ToLower(), @"[^a-z\d]", "-");
            var volumes = _volumes.Select((v, i) => new V1Volume
            {
                Name = $"v{i}",
                HostPath = new V1HostPathVolumeSource
                {
                    Type = "Directory",
                    Path = v.host,
                }
            }).ToArray();
            var volumeMounts = _volumes.Select((v, i) => new V1VolumeMount
            {
                Name = $"v{i}",
                MountPath = v.mount,
                                            
            }).ToArray();
            
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
                    BackoffLimit = 0,
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
                                    Command = jt.Arguments,
                                    VolumeMounts = volumeMounts,
                                    Resources = new V1ResourceRequirements
                                    {
                                        Limits = new Dictionary<string, ResourceQuantity>()
                                        {
                                            ["cpu"] = new ResourceQuantity(jt.Threads.ToString())
                                        } 
                                    }
                                }
                            },
                            Volumes = volumes,
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