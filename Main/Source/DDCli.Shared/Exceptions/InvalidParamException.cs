using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class InvalidParamException : Exception
    {
        public InvalidParamException() : base() { }
        public InvalidParamException(string message) : base(message) { }
    }
}
