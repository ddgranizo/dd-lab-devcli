using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.DD
{
    public class UpdateParameterCommand : CommandBase
    {

        public CommandParameterDefinition CommandKeyParameter { get; set; }

        public CommandParameterDefinition CommandValueParameter { get; set; }

        public static string HelpDefinition { get; private set; } = "Update existing parameter with new value";
        public IStoredDataService StoredDataService { get; }

        public UpdateParameterCommand(
            IStoredDataService storedDataService)
            : base(typeof(UpdateParameterCommand).Namespace, nameof(UpdateParameterCommand), HelpDefinition)
        {
            CommandKeyParameter = new CommandParameterDefinition("key",
                CommandParameterDefinition.TypeValue.String,
                "Key for the parameter. For replace with the value use [[key]]", "k");

            CommandValueParameter = new CommandParameterDefinition("value",
                CommandParameterDefinition.TypeValue.String,
                "Value which will be replaced when used the key", "v");
           
            RegisterCommandParameter(CommandKeyParameter);
            RegisterCommandParameter(CommandValueParameter);

            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandKeyParameter.Name)
                && IsParamOk(parameters, CommandValueParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var key = GetStringParameterValue(parameters, CommandKeyParameter.Name);
            var value = GetStringParameterValue(parameters, CommandValueParameter.Name);

            if (!StoredDataService.ExistsParameter(key))
            {
                throw new ParameterNotFoundException(key);
            }
            StoredDataService.UpdateParameter(key, value);

            Log($"Updated parameter!");
        }
    }
}
