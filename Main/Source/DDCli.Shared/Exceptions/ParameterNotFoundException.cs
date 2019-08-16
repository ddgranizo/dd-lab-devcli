using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class ParameterNotFoundException : Exception
    {
        public ParameterNotFoundException() : base() { }
        public ParameterNotFoundException(string message) : base(message) { }
    }
}
