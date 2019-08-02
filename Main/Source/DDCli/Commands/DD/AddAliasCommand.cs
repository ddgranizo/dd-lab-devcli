using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.DD
{
    public class AddAliasCommand : CommandBase
    {



        public CommandParameterDefinition CommandNameParameter { get; set; }

        public CommandParameterDefinition CommandAliasParameter { get; set; }
        public static string HelpDefinition { get; private set; } = "Add alias for invoce command";
        public IStoredDataService StoredDataService { get; }
        public List<CommandBase> RegisteredCommands { get; }

        public AddAliasCommand(
            IStoredDataService storedDataService,
            List<CommandBase> registeredCommands)
            : base(typeof(AddAliasCommand).Namespace, nameof(AddAliasCommand), HelpDefinition)
        {
            CommandNameParameter = new CommandParameterDefinition("command",
                CommandParameterDefinition.TypeValue.String,
                "Command name for the alias");

            CommandAliasParameter = new CommandParameterDefinition("alias",
                CommandParameterDefinition.TypeValue.String,
                "Alias for the command");

            RegisterCommandParameter(CommandNameParameter);
            RegisterCommandParameter(CommandAliasParameter);

            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));
            RegisteredCommands = registeredCommands ?? throw new ArgumentNullException(nameof(registeredCommands));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandNameParameter.Name)
                && IsParamOk(parameters, CommandAliasParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var commandName = GetStringParameterValue(parameters, CommandNameParameter.Name);
            var aliasName = GetStringParameterValue(parameters, CommandAliasParameter.Name);

            var commandAliased = RegisteredCommands
                .FirstOrDefault(k => k.CommandName.ToLowerInvariant() == commandName);

            if (commandAliased == null)
            {
                throw new CommandNotFoundException(commandName);
            }

            if (StoredDataService.ExistsAlias(aliasName))
            {
                throw new AliasRepeatedException(aliasName);
            }

            StoredDataService.AddAlias(commandName, aliasName);

            Log($"Alias '{aliasName}' stored for command '{commandName}'");
        }
    }
}
