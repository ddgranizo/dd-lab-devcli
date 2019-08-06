using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Models
{
    public class CommandAlias
    {
        public CommandAlias()
        {

        }
        public CommandAlias(string commandName, string alias)
        {
            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentException("message", nameof(commandName));
            }

            if (string.IsNullOrEmpty(alias))
            {
                throw new ArgumentException("message", nameof(alias));
            }

            CommandName = commandName;
            Alias = alias;
        }

        public string CommandName { get; set; }
        public string Alias { get; set; }
    }
}
