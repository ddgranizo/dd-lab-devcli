using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class CommandBase
    {
        public string CommandName { get; set; }
        public string CommandNameSpace { get; set; }
        public string Help { get; set; }
        public CommandBase(string commandNameSpace, string commandName, string help)
        {
            if (string.IsNullOrEmpty(commandNameSpace))
            {
                throw new ArgumentException("message", nameof(commandNameSpace));
            }

            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentException("message", nameof(commandName));
            }

            if (string.IsNullOrEmpty(help))
            {
                throw new ArgumentException("message", nameof(help));
            }

            CommandNameSpace = commandNameSpace;
            CommandName = commandName;
            Help = help;
        }
    }
}
