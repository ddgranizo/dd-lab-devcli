using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Windows
{
    public class ReplaceFileContentCommand : CommandBase
    {
        private const string HelpDefinition = @"replace text in all files in path";


        public CommandParameterDefinition CommandPathParameter { get; set; }
        public CommandParameterDefinition CommandOldValueParameter { get; set; }
        public CommandParameterDefinition CommandNewValueParameter { get; set; }
        public CommandParameterDefinition CommandPatternParameter { get; set; }
        public IFileService FileService { get; }

        public ReplaceFileContentCommand(
            IFileService fileService)
            : base(typeof(ReplaceFileContentCommand).Namespace, nameof(ReplaceFileContentCommand), HelpDefinition)
        {
           
            CommandPathParameter = new CommandParameterDefinition(
                "path",
                CommandParameterDefinition.TypeValue.String,
                "Indicates the path (file or directory) for replacing",
                "p");

            CommandOldValueParameter = new CommandParameterDefinition(
                "oldvalue",
                CommandParameterDefinition.TypeValue.String,
                "Old value for replace",
                "o");

            CommandNewValueParameter = new CommandParameterDefinition(
                "newvalue",
                CommandParameterDefinition.TypeValue.String,
                "New value for replace",
                "n");

            CommandPatternParameter = new CommandParameterDefinition(
                "pattern",
                CommandParameterDefinition.TypeValue.String,
                "Pattern for files (use more than one with ;). Default value: *.*",
                "a");


            CommandParametersDefinition.Add(CommandPathParameter);
            CommandParametersDefinition.Add(CommandOldValueParameter);
            CommandParametersDefinition.Add(CommandNewValueParameter);
            CommandParametersDefinition.Add(CommandPatternParameter);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name)
                    && IsParamOk(parameters, CommandOldValueParameter.Name)
                    && IsParamOk(parameters, CommandNewValueParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var path = GetStringParameterValue(parameters, CommandPathParameter.Name);
            var oldValue = GetStringParameterValue(parameters, CommandOldValueParameter.Name);
            var newValue = GetStringParameterValue(parameters, CommandNewValueParameter.Name);

            string pattern = "*.*";
            if (IsParamOk(parameters, CommandPatternParameter.Name))
            {
                pattern = GetStringParameterValue(parameters, CommandPatternParameter.Name);
            }
            FileService.ReplaceFilesContents(path, oldValue, newValue, pattern);
        }

      
    }
}
