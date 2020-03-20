using DDCli.Dynamics.Utilities;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Commands
{
    public class CloneUsdConfigurationCommand : CommandBase
    {
        private const string HelpDefinition = "Clone USD configuration from one environment to other";

        public CommandParameterDefinition CommandStringConnectionFromMasterParameter { get; set; }
        public CommandParameterDefinition CommandStringConnectionFromSlaveParameter { get; set; }

        public CommandParameterDefinition EntitiesParameter { get; set; }



        public CloneUsdConfigurationCommand()
                : base(typeof(CloneUsdConfigurationCommand).Namespace, nameof(CloneUsdConfigurationCommand), HelpDefinition)
        {

            CommandStringConnectionFromMasterParameter = new CommandParameterDefinition("stringconnectionfrom",
                CommandParameterDefinition.TypeValue.String,
                "String connection for Dynamics for the master environment", "from");

            CommandStringConnectionFromSlaveParameter = new CommandParameterDefinition("stringconnectionto",
               CommandParameterDefinition.TypeValue.String,
               "String connection for Dynamics for the slave environment where the data will be written", "to");

            EntitiesParameter = new CommandParameterDefinition("entities",
                CommandParameterDefinition.TypeValue.String,
                "Entities splitted by comma ','. Remember include also intersections", "e");


            RegisterCommandParameter(CommandStringConnectionFromMasterParameter);
            RegisterCommandParameter(CommandStringConnectionFromSlaveParameter);
            RegisterCommandParameter(EntitiesParameter);

        }



        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandStringConnectionFromMasterParameter.Name)
                && IsParamOk(parameters, CommandStringConnectionFromSlaveParameter.Name)
                && IsParamOk(parameters, EntitiesParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var stringConnectionFrom = GetStringParameterValue(parameters, CommandStringConnectionFromMasterParameter.Name);
            var stringConnectionTo = GetStringParameterValue(parameters, CommandStringConnectionFromSlaveParameter.Name);
            var entities = GetStringParameterValue(parameters, EntitiesParameter.Name);

            var composedTo = $"{stringConnectionTo};RequireNewInstance=true";
            var composedFrom = $"{stringConnectionFrom};RequireNewInstance=true";
            var entitiesList = entities.Split(',');
            var serviceFrom = CrmProvider.GetService(composedFrom);
            var serviceTo = CrmProvider.GetService(composedTo);

            var displayFrom = CrmProvider.GetServiceDisplayName(composedFrom);
            var displayTo = CrmProvider.GetServiceDisplayName(composedTo);

            ConsoleService.WriteLine($"You are migrating data from '{displayFrom}' to '{displayTo}'. There will be operations of create, update and delete in the destionation environemtn '{displayTo}' and the operation cannot be undone. Confirm? (Y/N)");
            var response = ConsoleService.ReadLine();
            if (!string.IsNullOrEmpty(response))
            {
                var input = response.ToLowerInvariant();
                if (input == "y" || input == "yes")
                {
                    MigrationProvider.Migrate((string text) => { ConsoleService.WriteLine(text); }, serviceFrom, serviceTo, entitiesList);
                }
            }
        }
    }
}
