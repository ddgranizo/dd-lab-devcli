using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DDCli.Services
{
    public class LoggerService : ILoggerService
    {
        public LoggerService(bool interactive = true)
        {
            Interactive = interactive;
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
        }

        public bool Interactive { get; }

        public void Log(string text)
        {
            if (Interactive)
            {
                Console.WriteLine(text);
            }
            var filename = $"dd_{DateTime.Now.ToString("yyyyMMdd")}.log";
            var content = $"{DateTime.Now.ToString("yyyyMMdd hh:mm:ss:ffff")} - {text}";
            File.AppendAllLines($"Logs\\{filename}", new List<string>() { content });
        }
    }
}
