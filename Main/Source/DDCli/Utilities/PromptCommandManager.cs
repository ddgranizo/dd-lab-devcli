using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Utilities
{

    public delegate void CommandPromptLogHandler(string output);
    public static class PromptCommandManager
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



        public static void OpenExplorer(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        public static void RunCommand(string command, bool async = false)
        {
            var task = new Task(() =>
            {
                command.Bat();
            });
            if (async)
            {
                new System.Threading.Thread(() => { task.RunSynchronously(); }).Start();
            }
            else
            {
                task.RunSynchronously();
            }

        }

        public static void RunCommandConEmu(string command, bool async = false)
        {
            var task = new Task(() =>
            {
                command.BatConEmu();
            });
            if (async)
            {
                new System.Threading.Thread(() => { task.RunSynchronously(); }).Start();
            }
            else
            {
                task.RunSynchronously();
            }
        }

        public static void Run(string workingDirectory, string fileName, string parameters, bool asRoot = false, bool async = false)
        {
            var task = new Task(() =>
            {
                var proc1 = new ProcessStartInfo();
                proc1.UseShellExecute = true;
                if (asRoot)
                {
                    proc1.Verb = "runas";
                }
                proc1.WorkingDirectory = workingDirectory;
                if (!string.IsNullOrEmpty(fileName))
                {
                    proc1.FileName = fileName;
                }
                if (!string.IsNullOrEmpty(parameters))
                {
                    proc1.Arguments = parameters;
                }
                Process cmd = Process.Start(proc1);
                cmd.WaitForExit();
            });
            if (async)
            {
                new System.Threading.Thread(() => { task.RunSynchronously(); }).Start();
            }
            else
            {
                task.RunSynchronously();
            }

        }

    }
}
