using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Interfaces
{
    public interface IConsoleService
    {
        string ReadLine();

        void WriteLine(string text);
    }
}
