using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Events
{
    public class LogEventArgs : EventArgs
    {
        public LogEventArgs(string log)
        {
            Log = log;
        }

        public string Log { get; }
    }
}
