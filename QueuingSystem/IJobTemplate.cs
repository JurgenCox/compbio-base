using System.Collections.Generic;

namespace QueuingSystem
{
    public interface IJobTemplate
    {
        string ReadStderr();
        string ReadStdout();
        void Cleanup();
        Dictionary<string, string> JobEnvironment { get; set; }
        string InputPath { get; set; }
        string OutputPath { get; set; }
        string ErrorPath { get; set; }
        bool JoinFiles { get; set; }
        string[] Arguments { get; set; }
        string RemoteCommand { get; set; }
        string JobSubmissionState { get; set; }
        string WorkingDirectory { get; set; }
        string NativeSpecification { get; set; }
        string JobName { get; set; }
        
        string Submit();
    }
}