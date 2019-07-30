using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class InvalidParamsException : Exception
    {
        public InvalidParamsException() : base() { }
        public InvalidParamsException(string message) : base(message) { }
    }
}
