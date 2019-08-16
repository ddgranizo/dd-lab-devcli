using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class ParameterRepeatedException : Exception
    {
        public ParameterRepeatedException() : base() { }
        public ParameterRepeatedException(string message) : base(message) { }
    }
}
