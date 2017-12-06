using System.Text;
using System.Runtime.InteropServices;
using System;

namespace Drmaa {
    public class JobTemplate{
        private readonly DrmaaJobTemplate instance;

        public string JobEnvironment {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.JobEnvironment);
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.JobEnvironment, value);
            }
        }

        public string InputPath {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.InputPath);
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.InputPath, value);
            }
        }

        public string OutputPath {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.OutputPath);
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.OutputPath, value);
            }
        }

        public string ErrorPath {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.ErrorPath);
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.ErrorPath, value);
            }
        }

        public bool JoinFiles {
            get { 
                return DrmaaWrapper.DrmaaToBool(DrmaaWrapper.GetAttribute(instance, Attributes.JoinFiles));
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.JoinFiles, DrmaaWrapper.BoolToDrmaa(value));
            }
        }

        public string[] Arguments {
            get { 
                return DrmaaWrapper.GetAttributes(instance, Attributes.Argv); 
            }

            set { 
                DrmaaWrapper.SetAttributes(instance, Attributes.Argv, value); 
            }
        }

        public string RemoteCommand {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.RemoteCommand); 
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.RemoteCommand, value); 
            }
        }

        public string JobSubmissionState {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.JobSubmissionState); 
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.JobSubmissionState, value); 
            }
        }

        public string WorkingDirectory {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.WorkingDirectory); 
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.WorkingDirectory, value); 
            }
        }

        public string NativeSpecification {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.NativeSpecification); 
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.NativeSpecification, value); 
            }
        }

        public string JobName {
            get { 
                return DrmaaWrapper.GetAttribute(instance, Attributes.JobName); 
            }

            set { 
                DrmaaWrapper.SetAttribute(instance, Attributes.JobName, value); 
            }
        }

        internal JobTemplate(DrmaaJobTemplate instance){
            this.instance = instance;
        }

        public string Submit(){
            return DrmaaWrapper.RunJob(instance);
        }
    }
}