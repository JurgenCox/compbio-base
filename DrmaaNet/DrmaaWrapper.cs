using System;
using System.Collections.Generic;
using System.Text;

namespace DrmaaNet
{
    public class DrmaaException : Exception
    {
        public readonly int code;
        public readonly string message;

        public DrmaaException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }

        public override string ToString(){
            return $"code: {code}, message: \"{message}\"";
        }
    }

    public struct DrmaaJobTemplate
    {
        internal IntPtr instance;
    }

    public enum Action
    {
        Suspend = 0,
        Resume = 1,
        Hold = 2,
        Release = 3,
        Terminate = 4,
    }

    public enum Status {
        Undetermined = 0x00,
        QueuedActive = 0x10,
        SystemOnHold = 0x11,
        UserOnHold = 0x12,
        UserSystemOnHold = 0x13,
        Running = 0x20,
        SystemSuspended = 0x21,
        UserSuspended = 0x22,
        UserSystemSuspended = 0x23,
        Done = 0x30,
        Failed = 0x40,
    }
    
    internal static class Attributes {
        internal static readonly string RemoteCommand = "drmaa_remote_command";
        internal static readonly string WorkingDirectory = "drmaa_wd";
        internal static readonly string NativeSpecification = "drmaa_native_specification";
        internal static readonly string JobName = "drmaa_job_name";
        internal static readonly string JobSubmissionState = "drmaa_js_state";
        internal static readonly string Argv = "drmaa_v_argv";
        internal static readonly string InputPath = "drmaa_input_path";
        internal static readonly string OutputPath = "drmaa_output_path";
        internal static readonly string ErrorPath = "drmaa_error_path";
        internal static readonly string JoinFiles = "drmaa_join_files";
        internal static readonly string JobEnvironment = "drmaa_v_env";
    };

    internal static class DrmaaWrapper
    {
        internal const long WaitForever = -1;

        public static void Init(String contact)
        {
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_init(contact, error, error.Capacity);
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
        }

        public static void Exit(String contact)
        {
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_exit(error, error.Capacity);
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
        }

        public static DrmaaJobTemplate AllocateJobTemplate()
        {
            IntPtr instance;
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_allocate_job_template(out instance, error, error.Capacity);
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
            return new DrmaaJobTemplate { instance = instance };
        }

        public static string GetAttribute(DrmaaJobTemplate jobTemplate, string name)
        {
            StringBuilder value = new StringBuilder(1024);

            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_get_attribute(
                jobTemplate.instance,
                name,
                value,
                value.Capacity,
                error, error.Capacity
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
            return value.ToString();
        }

        public static void SetAttribute(DrmaaJobTemplate jobTemplate, string name, string value)
        {
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_set_attribute(
                jobTemplate.instance,
                name,
                value,
                error,
                error.Capacity
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
        }

        public static string RunJob(DrmaaJobTemplate jobTemplate)
        {
            StringBuilder jobId = new StringBuilder(1024);

            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_run_job(
                jobId,
                jobId.Capacity,
                jobTemplate.instance,
                error, error.Capacity
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
            return jobId.ToString();
        }

        public static void Control(string jobId, Action action){
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_control(
                jobId, (int)action,
                error, error.Capacity
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
        }

        public static Status JobPs(string jobId){
            StringBuilder error = new StringBuilder(1024);
            int status;

            int res = DrmaaWrapperInternal.drmaa_job_ps(
                jobId, out status,
                error, error.Capacity
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
            return (Status)status;
        }

        public static string[] GetAttributes(DrmaaJobTemplate jobTemplate, string name){
            IntPtr valuesIter;
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperInternal.drmaa_get_vector_attribute(
                jobTemplate.instance,
                name,
                out valuesIter,
                error, error.Capacity
            );

            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }

            StringBuilder value = new StringBuilder(1024);
            var result = new List<string>();
            while(true)
            {
                res = DrmaaWrapperInternal.drmaa_get_next_attr_value(
                    valuesIter,
                    value,
                    value.Capacity
                );
                if (res != 0)
                {
                    break;
                }
                result.Add(value.ToString());
            }
            DrmaaWrapperInternal.drmaa_release_attr_values(valuesIter);
            return result.ToArray();
        }

        public static void SetAttributes(DrmaaJobTemplate jobTemplate, string name, string[] values){
            StringBuilder error = new StringBuilder(1024);

            int res = DrmaaWrapperInternal.drmaa_set_vector_attribute(
                jobTemplate.instance,
                name,
                values,
                error, error.Capacity
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
        }

        public static bool DrmaaToBool(string drmaaBool){
            return drmaaBool == "y";
        }

        public static string BoolToDrmaa(bool nativeBool){
            return nativeBool ? "y" : "n";
        }

        public static Status Wait(string jobId, long timeout = -1)
        {
            StringBuilder error = new StringBuilder(1024);
            StringBuilder jobIdOut = new StringBuilder(1024);

            int stat;
            IntPtr rusage;
            int res = DrmaaWrapperInternal.drmaa_wait(
                jobId,
                jobIdOut, jobIdOut.Capacity,
                out stat, timeout, out rusage,
                error, error.Capacity
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
            int exitStatus;
            res = DrmaaWrapperInternal.drmaa_wexitstatus(
                out exitStatus, stat,
                error, error.Capacity  
            );
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
            return exitStatus == 0 ? Status.Done : Status.Failed;
        }

    }
}