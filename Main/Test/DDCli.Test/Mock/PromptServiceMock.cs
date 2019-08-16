using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class PromptServiceMock : IPromptCommandService
    {

        public string RunCommandValue { get; set; }
        public PromptServiceMock()
        {
        }

        public void OpenExplorer(string path)
        {
            throw new NotImplementedException();
        }

        public void Run(string workingDirectory, string fileName, string parameters, bool asRoot = false, bool async = false)
        {
            throw new NotImplementedException();
        }

        public void RunCommand(string command, bool async = false)
        {
            RunCommandValue = command;
        }

        public void RunCommandConEmu(string command, bool async = false)
        {
            throw new NotImplementedException();
        }

        public string ReadLine()
        {
            throw new NotImplementedException();
        }

        public void WriteLine(string text)
        {
            throw new NotImplementedException();
        }

        public string RunCommand(string command, string filename = null, string workingDirectory = null)
        {
            RunCommandValue = command;
            return string.Empty;
        }
    }
}
