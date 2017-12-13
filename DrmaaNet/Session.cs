using System;

namespace DrmaaNet{
    public static class Session
    {        
        private static bool _inited;
        
        
        public static void Init(string contact=null){
            if (_inited)
            {
                Console.Error.WriteLine("DRMAA session is already initialized");
                return;
            }
            //TODO: remove in release
            Console.WriteLine($"Initializing DRMAA session, contact: {contact}");
            DrmaaWrapper.Init(contact);
            _inited = true;
        }

        public static Status JobStatus(string jobId){
            return DrmaaWrapper.JobPs(jobId);
        }

        public static void JobControl(string jobId, Action action){
            DrmaaWrapper.Control(jobId, action);
        }

        public static JobTemplate AllocateJobTemplate(){
            return new JobTemplate(DrmaaWrapper.AllocateJobTemplate());
        }

        public static Status WaitForJobBlocking(string jobId, long timeout=DrmaaWrapper.WaitForever)
        {
            return DrmaaWrapper.Wait(jobId, timeout);
        }

        public static void Exit(string contact=null)
        {
            DrmaaWrapper.Exit(contact);
            _inited = false;
        }
    }
}