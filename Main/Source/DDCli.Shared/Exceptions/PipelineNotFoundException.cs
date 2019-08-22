using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class PipelineNotFoundException : Exception
    {
        public PipelineNotFoundException() : base() { }
        public PipelineNotFoundException(string message) : base(message) { }
    }
}
