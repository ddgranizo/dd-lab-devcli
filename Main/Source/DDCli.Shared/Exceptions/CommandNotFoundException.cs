using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class CommandNotFoundException : Exception
    {
        public CommandNotFoundException() : base() { }
        public CommandNotFoundException(string message) : base(message) { }
    }
}
