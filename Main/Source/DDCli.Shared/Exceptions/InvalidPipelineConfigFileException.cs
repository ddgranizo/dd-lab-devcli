using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class InvalidPipelineConfigFileException : Exception
    {
        public InvalidPipelineConfigFileException() : base() { }
        public InvalidPipelineConfigFileException(string message) : base(message) { }
    }
}
