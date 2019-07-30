using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Exceptions
{
    public class DuplicateCommandException : Exception
    {
        public List<string> Commands { get; set; }
        public DuplicateCommandException() : base() { }
        public DuplicateCommandException(List<string> commands) : base(string.Join(",", commands)) {
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }
    }
}
