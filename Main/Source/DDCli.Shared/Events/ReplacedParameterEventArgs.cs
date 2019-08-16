using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Events
{
    public class ReplacedParameterEventArgs : EventArgs
    {
        public ReplacedParameterEventArgs(string parameter)
        {
            Parameter = parameter;
        }

        public string Parameter { get; }
    }
}
