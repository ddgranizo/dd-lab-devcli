using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.DD
{
    public class DeleteParameterCommand : CommandBase
    {

        public CommandParameterDefinition CommandKeyParameter { get; set; }

        public static string HelpDefinition { get; private set; } = "Delete registered parameter";
        public IStoredDataService StoredDataService { get; }

        public DeleteParameterCommand(
            IStoredDataService storedDataService)
            : base(typeof(DeleteParameterCommand).Namespace, nameof(DeleteParameterCommand), HelpDefinition)
        {
            CommandKeyParameter = new CommandParameterDefinition("key",
                CommandParameterDefinition.TypeValue.String,
                "Parameter key for delete", "k");

            RegisterCommandParameter(CommandKeyParameter);
           
            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandKeyParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var key = GetStringParameterValue(parameters, CommandKeyParameter.Name);

            if (!StoredDataService.ExistsParameter(key))
            {
                throw new ParameterNotFoundException(key);
            }

            StoredDataService.DeleteParameter(key);
            Log($"Deleted parameter [[{key}]]");
        }
    }
}
