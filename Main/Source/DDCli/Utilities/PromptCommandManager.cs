using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace DDCli.Utilities
{

    public delegate void CommandPromptLogHandler(string output);
    public  class PromptCommandManager
    {
        public PromptCommandManager()
        {
        }

        public event CommandPromptLogHandler OnCommandPromptOutput;
        public void RunOld(string command)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;

            using (var process = System.Diagnostics.Process.Start(startInfo))
            {
                while (!process.HasExited)
                {
                    var output = process.StandardOutput.ReadToEnd();
                    OnCommandPromptOutput?.Invoke(output);
                }
            }
            
            //process.Start();
        }


        public void RunAs(string workingDirectory, string fileName, string parameters)
        {
            var proc1 = new ProcessStartInfo();
            proc1.UseShellExecute = true;
            proc1.Verb = "runas";
            proc1.WorkingDirectory = workingDirectory;
            proc1.FileName = fileName;
            proc1.Arguments = parameters;
            Process cmd = Process.Start(proc1);
            cmd.WaitForExit();
        }
        public void Run(string command)
        {
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = command;
            //startInfo.RedirectStandardOutput = true;
            //startInfo.UseShellExecute = false;
            System.Diagnostics.Process.Start(startInfo);
            //using (var process = System.Diagnostics.Process.Start(startInfo))
            //{
            //    //while (!process.HasExited)
            //    //{
            //    //    var output = process.StandardOutput.ReadToEnd();
            //    //    OnCommandPromptOutput?.Invoke(output);
            //    //}
            //}

            //process.Start();
        }
    }
}
