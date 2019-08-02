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

        public void RunCommand(string command, bool async = false)
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

        public void RunCommandConEmu(string command, bool async = false)
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
