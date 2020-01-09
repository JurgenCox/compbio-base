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
        string WorkingDirectory { get; set; }
        string JobName { get; set; }
        int Threads { get; set; }
        long MaxMemorySize { get; set; }
    }
}