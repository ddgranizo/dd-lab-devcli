using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.DD
{
    public class EchoCommand : CommandBase
    {
        public static string HelpDefinition { get; private set; } = "Echo input";


        public CommandParameterDefinition CommandTextParameter { get; set; }


        public EchoCommand()
            : base(typeof(EchoCommand).Namespace, nameof(EchoCommand), HelpDefinition)
        {
            CommandTextParameter = new CommandParameterDefinition("text",
                CommandParameterDefinition.TypeValue.String,
                "Text for echo", "t");

            RegisterCommandParameter(CommandTextParameter);
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandTextParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var text = GetStringParameterValue(parameters, CommandTextParameter.Name);
            Log(text);
        }
    }
}
