using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.Utils
{
    public class SetAlias : CommandBase
    {
        private const string HelpDefinition = "Set alias for user as shortcuts calling methods";


        public CommandParameterDefinition CommandNameParam { get; set; }
        public CommandParameterDefinition AliasParam { get; set; }
        public SetAlias()
            : base(typeof(SetAlias).Namespace, nameof(SetAlias), HelpDefinition)
        {
            CommandNameParam = new CommandParameterDefinition("command", CommandParameterDefinition.TypeValue.String, "Command for alias");
            AliasParam = new CommandParameterDefinition("alias", CommandParameterDefinition.TypeValue.String, "Alias");
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            if (!CheckAndExecuteHelpCommand(parameters))
            {
               
            }
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandNameParam.Name) && IsParamOk(parameters, AliasParam.Name);
        }
    }
}
