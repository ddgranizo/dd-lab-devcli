using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class InvalidZipFileException : Exception
    {
        public InvalidZipFileException() : base() { }
        public InvalidZipFileException(string message) : base(message) { }
    }
}
