using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class InvalidParamNameException : Exception
    {
        public InvalidParamNameException() : base() { }
        public InvalidParamNameException(string message) : base(message) { }
    }
}
