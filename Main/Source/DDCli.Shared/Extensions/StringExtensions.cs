using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DDCli.Extensions
{
    public static class StringExtensions
    {
        public static string Bat(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            string result = RunCommand("cmd.exe", $"/c \"{escapedArgs}\"");
            return result;
        }

        public static string BatConEmu(this string cmd)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            string result = RunCommand(@"C:\Program Files\ConEmu\ConEmu64.exe", $"\"{escapedArgs}\"");
            return result;
        }


        private static string RunCommand(string filename, string arguments)
        {
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = arguments,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            Console.WriteLine(result);
            process.WaitForExit();
            return string.Empty;//return result;
        }


    }
}
