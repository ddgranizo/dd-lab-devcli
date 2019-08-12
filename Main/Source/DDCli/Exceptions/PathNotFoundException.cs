using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class PathNotFoundException : Exception
    {
        public PathNotFoundException() : base() { }
        public PathNotFoundException(string message) : base(message) { }
    }
}
