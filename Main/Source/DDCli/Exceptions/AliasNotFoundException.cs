using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class AliasNotFoundException : Exception
    {
        public AliasNotFoundException() : base() { }
        public AliasNotFoundException(string message) : base(message) { }
    }
}
