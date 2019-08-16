using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Windows
{
    public class RenameFolderCommand : CommandBase
    {
        private const string HelpDefinition = @"rename folder";
        public CommandParameterDefinition CommandPathOldFolderParameter { get; set; }
        public CommandParameterDefinition CommandPathNewFolderParameter { get; set; }
        public IFileService FileService { get; }


        public RenameFolderCommand(
            IFileService fileService)
            : base(typeof(RenameFolderCommand).Namespace, nameof(RenameFolderCommand), HelpDefinition)
        {
            CommandPathOldFolderParameter = new CommandParameterDefinition(
                "oldpath",
                CommandParameterDefinition.TypeValue.String,
                "Source path for rename",
                "o");

            CommandPathNewFolderParameter = new CommandParameterDefinition(
                "newpath",
                CommandParameterDefinition.TypeValue.String,
                "New name of the folder",
                "d");

            CommandParametersDefinition.Add(CommandPathOldFolderParameter);
            CommandParametersDefinition.Add(CommandPathNewFolderParameter);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathOldFolderParameter.Name)
                && IsParamOk(parameters, CommandPathNewFolderParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var oldPath = GetStringParameterValue(parameters, CommandPathOldFolderParameter.Name);
            var nePath = GetStringParameterValue(parameters, CommandPathNewFolderParameter.Name);
            FileService.RenameFolder(oldPath, nePath);
        }
    }
}
