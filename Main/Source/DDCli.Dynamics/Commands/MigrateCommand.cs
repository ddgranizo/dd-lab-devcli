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
    public class MigrateCommand : CommandBase
    {
        private const string HelpDefinition = "Migrate entities from one environment to other";

        public CommandParameterDefinition CommandStringConnectionFromMasterParameter { get; set; }
        public CommandParameterDefinition CommandStringConnectionFromSlaveParameter { get; set; }

        public CommandParameterDefinition CommandEntitiesParameter { get; set; }



        public MigrateCommand()
                : base(typeof(MigrateCommand).Namespace, nameof(MigrateCommand), HelpDefinition)
        {

            CommandStringConnectionFromMasterParameter = new CommandParameterDefinition("stringconnectionfrom",
                CommandParameterDefinition.TypeValue.String,
                "String connection for Dynamics for the master environment", "from");

            CommandStringConnectionFromSlaveParameter = new CommandParameterDefinition("stringconnectionto",
               CommandParameterDefinition.TypeValue.String,
               "String connection for Dynamics for the slave environment where the data will be written", "to");

            CommandEntitiesParameter = new CommandParameterDefinition("entities",
                CommandParameterDefinition.TypeValue.String,
                "Entities for migrate splitted by ; (ie. contact;account). Include entity intersection for associate", "e");


            RegisterCommandParameter(CommandStringConnectionFromMasterParameter);
            RegisterCommandParameter(CommandStringConnectionFromSlaveParameter);
            RegisterCommandParameter(CommandEntitiesParameter);

        }



        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandStringConnectionFromMasterParameter.Name)
                && IsParamOk(parameters, CommandStringConnectionFromSlaveParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var stringConnectionFrom = GetStringParameterValue(parameters, CommandStringConnectionFromMasterParameter.Name);
            var stringConnectionTo = GetStringParameterValue(parameters, CommandStringConnectionFromSlaveParameter.Name);
            var includeOptions = GetBoolParameterValue(parameters, CommandEntitiesParameter.Name, false);

            var composedTo = $"{stringConnectionTo};RequireNewInstance=true";
            var composedFrom = $"{stringConnectionFrom};RequireNewInstance=true";

            var serviceFrom = CrmProvider.GetService(composedFrom);
            var serviceTo = CrmProvider.GetService(composedTo);

            var displayFrom = CrmProvider.GetServiceDisplayName(composedFrom);
            var displayTo = CrmProvider.GetServiceDisplayName(composedTo);

            ConsoleService.WriteLine($"You are migrating data from '{displayFrom}' to '{displayTo}'. The data in '{displayTo}' will be modified and the operation cannot be undone. Confirm? (Y/N)");
            var response = ConsoleService.ReadLine();
            if (!string.IsNullOrEmpty(response))
            {
                var input = response.ToLowerInvariant();
                if (input == "y" || input == "yes")
                {
                    CrmProvider.CloneUsdConfiguration((string text) => { ConsoleService.WriteLine(text); }, serviceFrom, serviceTo, includeOptions);
                }
            }
        }
    }
}
