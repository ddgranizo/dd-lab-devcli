using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class LoggerServiceMock : ILoggerService
    {
        public List<string> Logs { get; set; }
        public LoggerServiceMock()
        {
            Logs = new List<string>();
        }

        public void Log(string text)
        {
            Logs.Add(text);
            Console.WriteLine(text);
        }
    }
}
