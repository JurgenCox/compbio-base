using System;

namespace DrmaaNet{
    public static class Session
    {        
        // TODO:
        /// <summary>
        ///
        ///      -inherit
//        Available only for qrsh and qmake(1).
//
//        qrsh allows the user to start  a  task  in  an  already
//        scheduled parallel job.  The option -inherit tells qrsh
//            to read a job id from the environment  variable  JOB_ID
//            and  start the specified command as a task in this job.
//            Please note that in this case, the hostname of the host
//            where  the  command  will  be executed must precede the
//        command to execute; the syntax changes to
        /// </summary>
        private static bool _inited;
        
        public static void Init(string contact=null){
            if (_inited)
            {
                Console.Error.WriteLine("DRMAA session is already initialized");
                return;
            }
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