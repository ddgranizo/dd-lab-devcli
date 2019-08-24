using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Windows
{
    public class MovePathCommand : CommandBase
    {
        private const string HelpDefinition = @"move file or folder contain to other folder";
        public CommandParameterDefinition CommandSourcePathParameter { get; set; }
        public CommandParameterDefinition CommandPatternParameter { get; set; }
        public CommandParameterDefinition CommandDestinationFolderParameter { get; set; }
        public IFileService FileService { get; }


        public MovePathCommand(
            IFileService fileService)
            : base(typeof(MovePathCommand).Namespace, nameof(MovePathCommand), HelpDefinition)
        {
            CommandSourcePathParameter = new CommandParameterDefinition(
                "sourcepath",
                CommandParameterDefinition.TypeValue.String,
                "Source file or folder for move",
                "sp");

            CommandDestinationFolderParameter = new CommandParameterDefinition(
                "destionationfolder",
                CommandParameterDefinition.TypeValue.String,
                "destination folder",
                "df");

            CommandPatternParameter = new CommandParameterDefinition(
                "pattern",
                CommandParameterDefinition.TypeValue.String,
                "file pattern if move a folder. Default value: *.*",
                "p");

            CommandParametersDefinition.Add(CommandSourcePathParameter);
            CommandParametersDefinition.Add(CommandDestinationFolderParameter);
            CommandParametersDefinition.Add(CommandPatternParameter);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandSourcePathParameter.Name)
                && IsParamOk(parameters, CommandDestinationFolderParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {

            var sourcePath = GetStringParameterValue(parameters, CommandSourcePathParameter.Name);
            var destinationFolder = GetStringParameterValue(parameters, CommandDestinationFolderParameter.Name);

            if (!FileService.ExistsPath(sourcePath))
            {
                throw new PathNotFoundException(sourcePath);
            }
            if (!FileService.ExistsPath(destinationFolder))
            {
                throw new PathNotFoundException(destinationFolder);
            }

            if (FileService.IsDirectory(sourcePath))
            {
                var pattern = GetStringParameterValue(parameters, CommandPatternParameter.Name, "*.*");
                FileService.MoveFolderContent(sourcePath, destinationFolder, pattern);
            }
            else
            {
                FileService.MoveFile(sourcePath, destinationFolder);
            }
            
            
        }
    }
}
