using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class TemplateNotFoundException : Exception
    {
        public TemplateNotFoundException() : base() { }
        public TemplateNotFoundException(string message) : base(message) { }
    }
}
