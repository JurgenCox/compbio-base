using System.Collections.Generic;
using System.Linq;

namespace Drmaa {
    public class JobTemplate{
        private readonly DrmaaJobTemplate _instance;
        private readonly Dictionary<string, object> _attributesCache = new Dictionary<string, object>();

        public void InvalidateAttributesCache()
        {
            _attributesCache.Clear();
        }
        
        public Dictionary<string, string> JobEnvironment {
            get
            {
                var envStrings = GetAttributes(Attributes.JobEnvironment);
                return envStrings.Select(x => x.Split('=')).ToDictionary(x => x[0], y => y[1]);
            }

            set
            {
                var attrs = value.Select(x => $"{x.Key}={x.Value}").ToArray();
                SetAttributes(Attributes.JobEnvironment, attrs);
            }
        }

        public string InputPath {
            get { 
                return GetAttribute(Attributes.InputPath);
            }

            set { 
                SetAttribute(Attributes.InputPath, value);
            }
        }

        public string OutputPath {
            get { 
                return GetAttribute(Attributes.OutputPath);
            }

            set { 
                SetAttribute(Attributes.OutputPath, value);
            }
        }

        public string ErrorPath {
            get { 
                return GetAttribute(Attributes.ErrorPath);
            }

            set { 
                SetAttribute(Attributes.ErrorPath, value);
            }
        }

        public bool JoinFiles {
            get { 
                return DrmaaWrapper.DrmaaToBool(GetAttribute(Attributes.JoinFiles));
            }

            set { 
                SetAttribute(Attributes.JoinFiles, DrmaaWrapper.BoolToDrmaa(value));
            }
        }

        public string[] Arguments {
            get { 
                return GetAttributes( Attributes.Argv); 
            }

            set { 
                SetAttributes(Attributes.Argv, value); 
            }
        }

        public string RemoteCommand {
            get { 
                return GetAttribute(Attributes.RemoteCommand); 
            }

            set { 
                SetAttribute(Attributes.RemoteCommand, value); 
            }
        }

        public string JobSubmissionState {
            get { 
                return GetAttribute(Attributes.JobSubmissionState); 
            }

            set { 
                SetAttribute(Attributes.JobSubmissionState, value); 
            }
        }

        public string WorkingDirectory {
            get { 
                return GetAttribute(Attributes.WorkingDirectory); 
            }

            set { 
                SetAttribute(Attributes.WorkingDirectory, value); 
            }
        }

        public string NativeSpecification {
            get { 
                return GetAttribute(Attributes.NativeSpecification); 
            }

            set { 
                SetAttribute(Attributes.NativeSpecification, value); 
            }
        }

        public string JobName {
            get { 
                return GetAttribute(Attributes.JobName); 
            }

            set { 
                SetAttribute(Attributes.JobName, value); 
            }
        }
        
        private string GetAttribute(string name)
        {
            if (_attributesCache.ContainsKey(name))
            {
                return _attributesCache[name] as string;
            }
            return DrmaaWrapper.GetAttribute(_instance, name);
        } 
        
        private string[] GetAttributes(string name)
        {
            if (_attributesCache.ContainsKey(name))
            {
                return _attributesCache[name] as string[];
            }
            return DrmaaWrapper.GetAttributes(_instance, name);
        } 
        
        private void SetAttribute(string name, string value)
        {
            DrmaaWrapper.SetAttribute(_instance, name, value);
            _attributesCache[name] = value;
        }
        
        private void SetAttributes(string name, string[] value)
        {
            DrmaaWrapper.SetAttributes(_instance, name, value);
            _attributesCache[name] = value;
        } 
        
        
        internal JobTemplate(DrmaaJobTemplate instance){
            _instance = instance;
        }

        public string Submit(){
            return DrmaaWrapper.RunJob(_instance);
        }
    }
}