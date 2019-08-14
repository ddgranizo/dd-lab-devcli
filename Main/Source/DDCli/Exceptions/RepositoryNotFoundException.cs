using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class RepositoryNotFoundException : Exception
    {
        public RepositoryNotFoundException() : base() { }
        public RepositoryNotFoundException(string message) : base(message) { }
    }
}
