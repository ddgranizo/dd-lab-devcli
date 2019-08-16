using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class TemplateNameRepeatedException : Exception
    {
        public TemplateNameRepeatedException() : base() { }
        public TemplateNameRepeatedException(string message) : base(message) { }
    }
}
