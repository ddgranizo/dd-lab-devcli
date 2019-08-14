using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.DD
{
    public class WindowsCmdCommand : CommandBase
    {
        public static string HelpDefinition { get; private set; } = "Execute any cmd command";


        public CommandParameterDefinition CommandCmdParameter { get; set; }
        public IPromptCommandService PromptCommandService { get; }

        public WindowsCmdCommand(IPromptCommandService promptCommandService)
            : base(typeof(WindowsCmdCommand).Namespace, nameof(WindowsCmdCommand), HelpDefinition)
        {
            CommandCmdParameter = new CommandParameterDefinition("command",
                CommandParameterDefinition.TypeValue.String,
                "Command name for the alias", "c");

            RegisterCommandParameter(CommandCmdParameter);
            PromptCommandService = promptCommandService 
                ?? throw new ArgumentNullException(nameof(promptCommandService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandCmdParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var command = GetStringParameterValue(parameters, CommandCmdParameter.Name);
            PromptCommandService.RunCommand(command);
        }
    }
}
