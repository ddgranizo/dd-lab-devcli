using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.DD
{
    public class AddParameterCommand : CommandBase
    {

        public CommandParameterDefinition CommandKeyParameter { get; set; }

        public CommandParameterDefinition CommandValueParameter { get; set; }

        public CommandParameterDefinition CommandIsEncryptedParameter { get; set; }
        public static string HelpDefinition { get; private set; } = "Add parameter for use as replaced in any other command. Use it with [[my.parameter.name]]";
        public IStoredDataService StoredDataService { get; }

        public AddParameterCommand(
            IStoredDataService storedDataService)
            : base(typeof(AddParameterCommand).Namespace, nameof(AddParameterCommand), HelpDefinition)
        {
            CommandKeyParameter = new CommandParameterDefinition("key",
                CommandParameterDefinition.TypeValue.String,
                "Key for the parameter. For replace with the value use [[key]]", "k");

            CommandValueParameter = new CommandParameterDefinition("value",
                CommandParameterDefinition.TypeValue.String,
                "Value which will be replaced when used the key", "v");

            CommandIsEncryptedParameter = new CommandParameterDefinition("encrypted",
                CommandParameterDefinition.TypeValue.Boolean,
                "Encrypt the parameter (Use for store passwords)", "e");

            RegisterCommandParameter(CommandKeyParameter);
            RegisterCommandParameter(CommandValueParameter);
            RegisterCommandParameter(CommandIsEncryptedParameter);

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
            var isEncrypted = GetBoolParameterValue(parameters, CommandIsEncryptedParameter.Name);

            if (StoredDataService.ExistsParameter(key))
            {
                throw new ParameterRepeatedException(key);
            }

            StoredDataService.AddParameter(key, value, isEncrypted);
            var displayValue = isEncrypted ? "*****" : value;
            Log($"Added parameter [[{key}]]={displayValue}");
        }
    }
}
