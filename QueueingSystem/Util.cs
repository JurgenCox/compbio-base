using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace QueueingSystem
{
    public static class Util
    {
        public static string FormatTemplateString(string template, IDictionary<string, object> context)
        {
            foreach (var kv in context)
            {
                var key = kv.Key;
                var substitution = kv.Value;
                template = template.Replace($"{{{key}}}", (substitution ?? "").ToString());
            }

            return template;
        }
        
        private static IEnumerable<string> Split(this string str, 
            Func<char, bool> controller)
        {
            int nextPiece = 0;

            for (int c = 0; c < str.Length; c++)
            {
                if (controller(str[c]))
                {
                    yield return str.Substring(nextPiece, c - nextPiece);
                    nextPiece = c + 1;
                }
            }

            yield return str.Substring(nextPiece);
        }
        
        private static string TrimMatchingQuotes(this string input, char quote)
        {
            if ((input.Length >= 2) && 
                (input[0] == quote) && (input[input.Length - 1] == quote))
                return input.Substring(1, input.Length - 2);

            return input;
        }
        
        public static IEnumerable<string> SplitCommandLine(string commandLine)
        {
            bool inQuotes = false;

            return commandLine.Split(c =>
                {
                    if (c == '\"')
                        inQuotes = !inQuotes;

                    return !inQuotes && c == ' ';
                })
                .Select(arg => arg.Trim().TrimMatchingQuotes('\"'))
                .Where(arg => !String.IsNullOrEmpty(arg));
        }

        public static bool IsRunning(Process process) {
            if (process == null) return false;
            try {
                Process.GetProcessById(process.Id);
            } catch (Exception) {
                return false;
            }
            return true;
        }

        /// <summary>
        /// http://www.mono-project.com/docs/gui/winforms/porting-winforms-applications/
        /// </summary>
        public static bool IsRunningOnMono() => Type.GetType("Mono.Runtime") != null;

        public static string IntString(int x, int n) {
            int npos = (int) Math.Ceiling(Math.Log10(n));
            string result = "" + x;
            if (result.Length >= npos) {
                return result;
            }
            return Repeat(npos - result.Length, "0") + result;
        }

        private static string Repeat(int n, string s) {
            StringBuilder b = new StringBuilder();
            for (int i = 0; i < n; i++) {
                b.Append(s);
            }
            return b.ToString();
        }
    }
}