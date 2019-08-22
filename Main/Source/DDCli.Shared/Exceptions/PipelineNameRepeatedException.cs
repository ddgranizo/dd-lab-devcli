using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class PipelineNameRepeatedException : Exception
    {
        public PipelineNameRepeatedException() : base() { }
        public PipelineNameRepeatedException(string message) : base(message) { }
    }
}
