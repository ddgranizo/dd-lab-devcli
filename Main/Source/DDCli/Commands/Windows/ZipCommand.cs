using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Windows
{
    public class ZipCommand : CommandBase
    {
        private const string HelpDefinition = @"zip file or directory";


        public CommandParameterDefinition CommandPathParameter { get; set; }
        public IFileService FileService { get; }

        public ZipCommand(
            IFileService fileService)
            : base(typeof(ZipCommand).Namespace, nameof(ZipCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition(
                "path",
                CommandParameterDefinition.TypeValue.String,
                "Indicates the path (file or directory) for zip",
                "p");

            CommandParametersDefinition.Add(CommandPathParameter);
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var path = GetStringParameterValue(parameters, CommandPathParameter.Name);
            var isDirectory = FileService.IsDirectory(path);
            if (isDirectory)
            {
                FileService.ZipDierctory(path);
            }
            else
            {
                FileService.ZipFile(path);
            }
        }

      
    }
}
