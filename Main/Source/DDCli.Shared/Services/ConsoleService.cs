using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Services
{
    public class ConsoleService : IConsoleService
    {
        public ILoggerService LoggerService { get; }
        public List<string> ReturnValues { get; }
        public bool IsInteractive { get; set; }

        private int returnCounter = 0;

        public ConsoleService(ILoggerService loggerService, List<string> returnValues)
        {
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            ReturnValues = returnValues;
            IsInteractive = returnValues == null;
        }

        public string ReadLine()
        {
            if (IsInteractive)
            {
                return Console.ReadLine();
            }
            return ReturnValues[returnCounter++];
        }

        public void WriteLine(string text)
        {
            LoggerService.Log(text);
        }
    }
}
