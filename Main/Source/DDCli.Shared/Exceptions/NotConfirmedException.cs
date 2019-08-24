using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class NotConfirmedException : Exception
    {
        public NotConfirmedException() : base() { }
        public NotConfirmedException(string message) : base(message) { }
    }
}
