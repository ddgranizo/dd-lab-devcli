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
        public CommandParameterDefinition CommandWorkingDirectoryParameter { get; set; }

        public CommandParameterDefinition CommandFilenameParameter { get; set; }
        public IPromptCommandService PromptCommandService { get; }

        public WindowsCmdCommand(IPromptCommandService promptCommandService)
            : base(typeof(WindowsCmdCommand).Namespace, nameof(WindowsCmdCommand), HelpDefinition)
        {
            CommandCmdParameter = new CommandParameterDefinition("command",
                CommandParameterDefinition.TypeValue.String,
                "Command name for the alias", "c");

            CommandWorkingDirectoryParameter = new CommandParameterDefinition("workingdirectory",
                CommandParameterDefinition.TypeValue.String,
                "Working directory", "w");

            CommandFilenameParameter = new CommandParameterDefinition("filename",
                CommandParameterDefinition.TypeValue.String,
                "File name", "f");

            RegisterCommandParameter(CommandCmdParameter);
            RegisterCommandParameter(CommandWorkingDirectoryParameter);
            RegisterCommandParameter(CommandFilenameParameter);

            PromptCommandService = promptCommandService 
                ?? throw new ArgumentNullException(nameof(promptCommandService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandCmdParameter.Name)
                 || IsParamOk(parameters, CommandFilenameParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var workingDirectory = GetStringParameterValue(parameters, CommandWorkingDirectoryParameter.Name, null);
            var filename = GetStringParameterValue(parameters, CommandFilenameParameter.Name, null);
            var command = GetStringParameterValue(parameters, CommandCmdParameter.Name, null);
            if (command != null)
            {
                command = command.Replace("$$", "\"");
            }
            var response =  PromptCommandService.RunCommand(command, filename, workingDirectory);
            Log(response);
        }
    }
}
