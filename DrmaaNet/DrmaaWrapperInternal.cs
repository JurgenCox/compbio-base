using System;
using System.Runtime.InteropServices;
using System.Text;

namespace DrmaaNet
{
    internal static class DrmaaWrapperInternal
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
}