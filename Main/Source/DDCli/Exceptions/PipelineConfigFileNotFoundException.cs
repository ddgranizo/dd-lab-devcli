using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class PipelineConfigFileNotFoundException : Exception
    {
        public PipelineConfigFileNotFoundException() : base() { }
        public PipelineConfigFileNotFoundException(string message) : base(message) { }
    }
}
