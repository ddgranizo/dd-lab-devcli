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
    public class DeleteTemplateCommand : CommandBase
    {


        public CommandParameterDefinition CommandNameParameter { get; set; }
        public static string HelpDefinition { get; private set; } = "Deleted template from global access";
        public IStoredDataService StoredDataService { get; }
        public IFileService FileService { get; }
        public List<CommandBase> RegisteredCommands { get; }

        public DeleteTemplateCommand(
            IStoredDataService storedDataService)
            : base(typeof(DeleteTemplateCommand).Namespace, nameof(DeleteTemplateCommand), HelpDefinition)
        {
            CommandNameParameter = new CommandParameterDefinition("name",
                CommandParameterDefinition.TypeValue.String,
                "Template name", "n");
            
            RegisterCommandParameter(CommandNameParameter);

            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandNameParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var name = GetStringParameterValue(parameters, CommandNameParameter.Name);
            if (!StoredDataService.ExistsTemplate(name))
            {
                throw new TemplateNotFoundException();
            }
            StoredDataService.DeleteTemplate(name);
            Log($"Template deleted!");
        }
    }
}
