using DDCli.Dynamics.Utilities;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Commands
{
    public class GetTokenCommand : CommandBase
    {

        private const string HelpDefinition = "Return valid token for Dynamics instance";

        public CommandParameterDefinition CommandStringConnectionParameter { get; set; }

        public CommandParameterDefinition CommandClientIdParameter { get; set; }
        public GetTokenCommand()
                : base(typeof(GetTokenCommand).Namespace, nameof(GetTokenCommand), HelpDefinition)
        {
            CommandStringConnectionParameter = new CommandParameterDefinition("stringconnection",
                CommandParameterDefinition.TypeValue.String,
                "String connection for Dynamics", "s");

            CommandClientIdParameter = new CommandParameterDefinition("clientid",
               CommandParameterDefinition.TypeValue.String,
               "Client Id (or application Id) obtained from Azure AD", "c");

            RegisterCommandParameter(CommandStringConnectionParameter);
            RegisterCommandParameter(CommandClientIdParameter);
        }



        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandStringConnectionParameter.Name) 
                && IsParamOk(parameters, CommandClientIdParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var stringConnection = GetStringParameterValue(parameters, CommandStringConnectionParameter.Name);
            var clientId = GetStringParameterValue(parameters, CommandClientIdParameter.Name);
            var crmServiceClient = CrmProvider.GetCrmServiceClient(stringConnection);
            var proxy = crmServiceClient.OrganizationServiceProxy;
            var username = proxy.ClientCredentials.UserName.UserName;
            var password = proxy.ClientCredentials.UserName.Password;
            var host = $"https://{crmServiceClient.CrmConnectOrgUriActual.Host}/";
            var token = CrmProvider.GetCrmToken(username, password, host, clientId);
            Log(token);
        }
    }
}
