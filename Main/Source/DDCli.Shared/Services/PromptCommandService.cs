using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Utilities;

namespace DDCli.Services
{
    public class PromptCommandService : IPromptCommandService
    {
        public PromptCommandService()
        {
        }


        public void OpenExplorer(string path)
        {
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        public string RunCommand(string command, string filename = null, string workingDirectory = null)
        {
            return Bat(command, filename, workingDirectory);
            //var task = new Task(() =>
            //{

            //});
            //if (async)
            //{
            //    new System.Threading.Thread(() => { task.RunSynchronously(); }).Start();
            //}
            //else
            //{
            //    task.RunSynchronously();
            //}
            //return "";
        }


        public string Bat(string cmd, string filename = null, string workingDirectory = null)
        {
            var escapedArgs = cmd.Replace("\"", "\\\"");
            var file = filename ?? "cmd.exe";
            var cmdString = filename == null
                ? $"/c \"{escapedArgs}\""
                : escapedArgs;
            string result = InvokeRunCommand(file, cmdString, workingDirectory);
            return result;
        }

        //public  string BatConEmu(this string cmd)
        //{
        //    var escapedArgs = cmd.Replace("\"", "\\\"");
        //    string result = InvokeRunCommand(@"C:\Program Files\ConEmu\ConEmu64.exe", $"\"{escapedArgs}\"");
        //    return result;
        //}


        private string InvokeRunCommand(string filename, string arguments, string workingDirectory = null)
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
            if (workingDirectory != null)
            {
                process.StartInfo.WorkingDirectory = workingDirectory;
            }
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            //Console.WriteLine(result);
            process.WaitForExit();
            return result;//return result;
        }

        //public void RunCommandConEmu(string command, bool async = false)
        //{
        //    var task = new Task(() =>
        //    {
        //        command.BatConEmu();
        //    });
        //    if (async)
        //    {
        //        new System.Threading.Thread(() => { task.RunSynchronously(); }).Start();
        //    }
        //    else
        //    {
        //        task.RunSynchronously();
        //    }
        //}
        public void Run(string workingDirectory, string fileName, string parameters, bool asRoot = false, bool async = false)
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
