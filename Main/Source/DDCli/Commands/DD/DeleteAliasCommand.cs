using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.DD
{
    public class DeleteAliasCommand : CommandBase
    {

        public CommandParameterDefinition CommandAliasParameter { get; set; }
        public static string HelpDefinition { get; private set; } = "Delete alias";
        public IStoredDataService StoredDataService { get; }

        public DeleteAliasCommand(
            IStoredDataService storedDataService)
            : base(typeof(DeleteAliasCommand).Namespace, nameof(DeleteAliasCommand), HelpDefinition)
        {

            CommandAliasParameter = new CommandParameterDefinition("alias",
                CommandParameterDefinition.TypeValue.String,
                "Alias name", "a");

            RegisterCommandParameter(CommandAliasParameter);

            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandAliasParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var aliasName = GetStringParameterValue(parameters, CommandAliasParameter.Name);


            if (!StoredDataService.ExistsAlias(aliasName))
            {
                throw new AliasNotFoundException(aliasName);
            }

            StoredDataService.DeleteAlias(aliasName);
           
            Log($"Alias '{aliasName}' removed");
        }
    }
}
