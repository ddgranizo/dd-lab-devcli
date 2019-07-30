using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class NotArgumentsException : Exception
    {
        public NotArgumentsException() : base() { }
        public NotArgumentsException(string message) : base(message) { }
    }
}
