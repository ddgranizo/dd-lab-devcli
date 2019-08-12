using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class InvalidTemplateConfigFileException : Exception
    {
        public InvalidTemplateConfigFileException() : base() { }
        public InvalidTemplateConfigFileException(string message) : base(message) { }
    }
}
