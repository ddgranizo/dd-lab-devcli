using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class InvalidStringFormatException : Exception
    {
        public InvalidStringFormatException() : base() { }
        public InvalidStringFormatException(string message) : base(message) { }
    }
}
