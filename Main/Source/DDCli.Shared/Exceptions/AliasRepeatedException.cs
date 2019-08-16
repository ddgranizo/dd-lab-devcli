using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class AliasRepeatedException : Exception
    {
        public AliasRepeatedException() : base() { }
        public AliasRepeatedException(string message) : base(message) { }
    }
}
