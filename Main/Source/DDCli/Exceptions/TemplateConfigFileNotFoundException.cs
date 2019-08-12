using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class TemplateConfigFileNotFoundException : Exception
    {
        public TemplateConfigFileNotFoundException() : base() { }
        public TemplateConfigFileNotFoundException(string message) : base(message) { }
    }
}
