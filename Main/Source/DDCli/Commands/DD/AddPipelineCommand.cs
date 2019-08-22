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
    public class AddPipelineCommand : CommandBase
    {


        public CommandParameterDefinition CommandPathParameter { get; set; }
        public CommandParameterDefinition CommandNameParameter { get; set; }
        public CommandParameterDefinition CommandDescriptionParameter { get; set; }
        public static string HelpDefinition { get; private set; } = "Add pipeline path for global access";
        public IStoredDataService StoredDataService { get; }
        public IFileService FileService { get; }
        public List<CommandBase> RegisteredCommands { get; }

        public AddPipelineCommand(
            IStoredDataService storedDataService,
            IFileService fileService)
            : base(typeof(AddPipelineCommand).Namespace, nameof(AddPipelineCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition("path",
                CommandParameterDefinition.TypeValue.String,
                "Path to the pipeline json file", "p");

            CommandNameParameter = new CommandParameterDefinition("name",
                CommandParameterDefinition.TypeValue.String,
                "Pipeline name", "n");

            CommandDescriptionParameter = new CommandParameterDefinition("description",
               CommandParameterDefinition.TypeValue.String,
               "Pipeline description", "d");

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

            if (StoredDataService.ExistsPipeline(name))
            {
                throw new PipelineNameRepeatedException();
            }
            if (!FileService.ExistsFile(path))
            {
                throw new PathNotFoundException(path);
            }
            if (!StringFormats.IsValidLogicalName(name))
            {
                throw new InvalidStringFormatException("Name can only contains alphanumeric characters");
            }

            StoredDataService.AddPipeline(path, name, description);

            Log($"Pipeline stored!");
        }
    }
}
