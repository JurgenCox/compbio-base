using QueuingSystem.Drmaa;

namespace QueuingSystem
{
    public enum Action
    {
        Suspend,
        Resume,
        Terminate,
    }

    public enum Status {
        Unknown,
        Queued,
        Success,
        Failed,
        Running
    }
    
    public interface ISession
    {
        Status JobStatus(string jobId);

        void JobControl(string jobId, Action action);

        IJobTemplate AllocateJobTemplate();

        Status WaitForJobBlocking(string jobId);

        void Exit(string contact = null);
    }
}