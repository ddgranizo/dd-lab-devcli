using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands
{
    
    public class HelpCommand : CommandBase
    {

        private const string HelpDefinition = "Shows all the available commands";

        public HelpCommand()
            : base(nameof(HelpCommand), typeof(HelpCommand).Namespace, HelpDefinition)
        {

        }


        public override void Execute(List<CommandParameter> parameters)
        {
            
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }
    }
}
