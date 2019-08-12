using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class ConsoleServiceMock : IConsoleService
    {
        public string ReadLineReturn { get; set; }
        public List<string> ReadLineReturns { get; set; }

        private int _iterator = 0;
        public ConsoleServiceMock()
        {
        }

        public string ReadLine()
        {
            return ReadLineReturns == null ? ReadLineReturn : ReadLineReturns[_iterator++];
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text);
        }
    }
}
