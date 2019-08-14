using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Events
{
    public class ReplacedParameterValueEventArgs : EventArgs
    {
        public ReplacedParameterValueEventArgs(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }
}
