using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class NotValidCommandNameException : Exception
    {

        public NotValidCommandNameException() : base() { }
        public NotValidCommandNameException(string message):base (message) { }
    }
}
