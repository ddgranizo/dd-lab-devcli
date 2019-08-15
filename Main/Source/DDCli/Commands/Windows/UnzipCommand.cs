using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Windows
{
    public class UnzipCommand : CommandBase
    {
        private const string HelpDefinition = @"unzip file or directory";
        public CommandParameterDefinition CommandPathParameter { get; set; }
        public CommandParameterDefinition CommandDestinationParameter { get; set; }
        public IFileService FileService { get; }

        public UnzipCommand(
            IFileService fileService)
            : base(typeof(UnzipCommand).Namespace, nameof(UnzipCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition(
                "path",
                CommandParameterDefinition.TypeValue.String,
                "Indicates the path (file) for unzip",
                "p");

            CommandDestinationParameter = new CommandParameterDefinition(
                "destination",
                CommandParameterDefinition.TypeValue.String,
                "Indicates the destination folder",
                "d");

            CommandParametersDefinition.Add(CommandPathParameter);
            CommandParametersDefinition.Add(CommandDestinationParameter);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var path = GetStringParameterValue(parameters, CommandPathParameter.Name);
            string destinationFolder = null;
            if (IsParamOk(parameters, CommandDestinationParameter.Name))
            {
                destinationFolder = GetStringParameterValue(parameters, CommandDestinationParameter.Name);
            }
            var isFile = FileService.IsFile(path);
            if (!isFile)
            {
                throw new InvalidZipFileException();
            }
            FileService.UnZipPath(path, destinationFolder);
        }

      
    }
}
