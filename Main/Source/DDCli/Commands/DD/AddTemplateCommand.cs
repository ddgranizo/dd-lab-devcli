using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.DD
{
    public class AddTemplateCommand : CommandBase
    {


        public CommandParameterDefinition CommandPathParameter { get; set; }
        public CommandParameterDefinition CommandNameParameter { get; set; }
        public CommandParameterDefinition CommandDescriptionParameter { get; set; }
        public static string HelpDefinition { get; private set; } = "Add template path for global access";
        public IStoredDataService StoredDataService { get; }
        public IFileService FileService { get; }
        public List<CommandBase> RegisteredCommands { get; }

        public AddTemplateCommand(
            IStoredDataService storedDataService,
            IFileService fileService)
            : base(typeof(AddTemplateCommand).Namespace, nameof(AddTemplateCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition("path",
                CommandParameterDefinition.TypeValue.String,
                "Path to the template with the ddtemplate.json", "p");

            CommandNameParameter = new CommandParameterDefinition("name",
                CommandParameterDefinition.TypeValue.String,
                "Template name", "n");

            CommandDescriptionParameter = new CommandParameterDefinition("description",
               CommandParameterDefinition.TypeValue.String,
               "Template description", "d");

            RegisterCommandParameter(CommandPathParameter);
            RegisterCommandParameter(CommandNameParameter);
            RegisterCommandParameter(CommandDescriptionParameter);

            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name)
                && IsParamOk(parameters, CommandNameParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var path = GetStringParameterValue(parameters, CommandPathParameter.Name);
            var name = GetStringParameterValue(parameters, CommandNameParameter.Name);
            var description = GetStringParameterValue(parameters, CommandDescriptionParameter.Name);

            if (StoredDataService.ExistsTemplate(name))
            {
                throw new TemplateNameRepeatedException();
            }
            if (!FileService.ExistsDirectory(path))
            {
                throw new PathNotFoundException(path);
            }
            if (!FileService.ExistsTemplateConfigFile(path))
            {
                throw new TemplateConfigFileNotFoundException(path);
            }
            if (!StringFormats.IsValidLogicalName(name))
            {
                throw new InvalidStringFormatException("Name can only contains alphanumeric characters");
            }

            StoredDataService.AddTemplate(path, name, description);

            Log($"Template stored!");
        }
    }
}
