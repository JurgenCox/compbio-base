using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Drmaa
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

    internal class DrmaaWrapperIntenal
    {
        public static readonly int NO_MORE_ELEMENTS = 25;
        public static readonly int DRMAA_CONTROL_SUSPEND = 0;
        public static readonly int DRMAA_CONTROL_RESUME = 1;
        public static readonly int DRMAA_CONTROL_HOLD = 2;
        public static readonly int DRMAA_CONTROL_RELEASE = 3;
        public static readonly int DRMAA_CONTROL_TERMINATE = 4;

        public static int  DRMAA_PS_UNDETERMINED = 0x00;
        public static int  DRMAA_PS_QUEUED_ACTIVE = 0x10;
        public static int  DRMAA_PS_SYSTEM_ON_HOLD = 0x11;
        public static int  DRMAA_PS_USER_ON_HOLD = 0x12;
        public static int  DRMAA_PS_USER_SYSTEM_ON_HOLD = 0x13;
        public static int  DRMAA_PS_RUNNING = 0x20;
        public static int  DRMAA_PS_SYSTEM_SUSPENDED = 0x21;
        public static int  DRMAA_PS_USER_SUSPENDED = 0x22;
        public static int  DRMAA_PS_USER_SYSTEM_SUSPENDED = 0x23;
        public static int  DRMAA_PS_DONE = 0x30;
        public static int  DRMAA_PS_FAILED = 0x40;


        [DllImport("libdrmaa")]
        public static extern int drmaa_init(String contact, StringBuilder error_diagnosis, int error_diag_len);

        [DllImport("libdrmaa")]
        public static extern int drmaa_exit(StringBuilder error_diagnosis, int error_diag_len);


        
        [DllImport("libdrmaa")]
        public static extern int drmaa_allocate_job_template(out IntPtr jt, StringBuilder error_diagnosis, int error_diag_len);

        [DllImport("libdrmaa")]
        public static extern int drmaa_get_attribute_names(
                                    out IntPtr values,
                                    StringBuilder error_diagnosis, int error_diag_len
                                    );

        [DllImport("libdrmaa")]
        public static extern int drmaa_get_next_attr_name(
            IntPtr values,
            StringBuilder value, int value_len);

        [DllImport("libdrmaa")]
        public static extern int drmaa_get_attribute(
            IntPtr jt,
            string name, StringBuilder value, int value_len,
            StringBuilder error_diagnosis, int error_diag_len
            );

        [DllImport("libdrmaa")]
        public static extern int drmaa_set_attribute(
            IntPtr jt,
            string name, string value,
            StringBuilder error_diagnosis, int error_diag_len
            );

        [DllImport("libdrmaa")]
        public static extern int drmaa_run_job(
            StringBuilder job_id, int job_id_len, IntPtr jt,
            StringBuilder error_diagnosis, int error_diag_len
        );

        [DllImport("libdrmaa")]
        public static extern int drmaa_control(
            string job_id, int action,
            StringBuilder error_diagnosis, int error_diag_len
        );

        [DllImport("libdrmaa")]
        public static extern int drmaa_job_ps(
            string job_id, out int remote_ps,
            StringBuilder error_diagnosis, int error_diag_len
        );

        [DllImport("libdrmaa")]
        public static extern int drmaa_set_vector_attribute(
            IntPtr jt,
            string name, string[] values,
            StringBuilder error_diagnosis, int error_diag_len
        );

        [DllImport("libdrmaa")]
        public static extern int drmaa_get_vector_attribute(
            IntPtr jt,
            string name, out IntPtr values,
            StringBuilder error_diagnosis, int error_diag_len
        );

        [DllImport("libdrmaa")]
        public static extern int drmaa_get_next_attr_value( 
            IntPtr values,
            StringBuilder value, int value_len 
        );

        [DllImport("libdrmaa")]
        public static extern void drmaa_release_attr_values( 
            IntPtr values 
        );
        
        [DllImport("libdrmaa")]
        public static extern int drmaa_wait(
            string job_id,
            StringBuilder job_id_out, int job_id_out_len,
            out int stat, long timeout, out IntPtr rusage,
            StringBuilder error_diagnosis, int error_diag_len
        );
        
        [DllImport("libdrmaa")]
        public static extern int drmaa_wexitstatus( 
            out int exit_status, int stat,
            StringBuilder error_diagnosis, int error_diag_len
        );

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
    
    internal class Attributes {
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

    public class DrmaaWrapper
    {
        public static void Init(String contact)
        {
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperIntenal.drmaa_init(contact, error, error.Capacity);
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
        }

        public static void Exit(String contact)
        {
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperIntenal.drmaa_exit(error, error.Capacity);
            if (res != 0)
            {
                throw new DrmaaException(res, error.ToString());
            }
        }

        public static DrmaaJobTemplate AllocateJobTemplate()
        {
            IntPtr instance;
            StringBuilder error = new StringBuilder(1024);
            int res = DrmaaWrapperIntenal.drmaa_allocate_job_template(out instance, error, error.Capacity);
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
            int res = DrmaaWrapperIntenal.drmaa_get_attribute(
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
            int res = DrmaaWrapperIntenal.drmaa_set_attribute(
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
            int res = DrmaaWrapperIntenal.drmaa_run_job(
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
            int res = DrmaaWrapperIntenal.drmaa_control(
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

            int res = DrmaaWrapperIntenal.drmaa_job_ps(
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
            int res = DrmaaWrapperIntenal.drmaa_get_vector_attribute(
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
                res = DrmaaWrapperIntenal.drmaa_get_next_attr_value(
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
            DrmaaWrapperIntenal.drmaa_release_attr_values(valuesIter);
            return result.ToArray();
        }

        public static void SetAttributes(DrmaaJobTemplate jobTemplate, string name, string[] values){
            StringBuilder error = new StringBuilder(1024);

            int res = DrmaaWrapperIntenal.drmaa_set_vector_attribute(
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

        public static Status Wait(string jobId, int timeout = -1)
        {
            StringBuilder error = new StringBuilder(1024);
            StringBuilder jobIdOut = new StringBuilder(1024);

            int stat;
            IntPtr rusage;
            int res = DrmaaWrapperIntenal.drmaa_wait(
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
            res = DrmaaWrapperIntenal.drmaa_wexitstatus(
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