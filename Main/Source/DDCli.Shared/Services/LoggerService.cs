using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Services
{
    public class LoggerService : ILoggerService
    {
        private static readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        public LoggerService(bool interactive = true)
        {
            Interactive = interactive;
        }

        public bool Interactive { get; }

        public void Log(string text)
        {
            if (Interactive)
            {
                Console.WriteLine();
            }
            _logger.Info(text);
        }
    }
}
