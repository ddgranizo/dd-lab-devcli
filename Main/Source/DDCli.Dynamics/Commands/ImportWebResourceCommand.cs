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
    public class ImportWebResourceCommand : CommandBase
    {

        private const string HelpDefinition = "Imports web resource to Dynamics";

        public CommandParameterDefinition CommandPathParameter { get; set; }

        public CommandParameterDefinition CommandStringConnectionParameter { get; set; }

        public CommandParameterDefinition CommandResourceName { get; set; }

        public IFileService FileService { get; }

        public ImportWebResourceCommand(IFileService fileService)
                : base(typeof(ImportWebResourceCommand).Namespace, nameof(ImportWebResourceCommand), HelpDefinition)
        {

            CommandPathParameter = new CommandParameterDefinition("path",
                CommandParameterDefinition.TypeValue.String,
                "Path with the resource", "p");

            CommandStringConnectionParameter = new CommandParameterDefinition("stringconnection",
                CommandParameterDefinition.TypeValue.String,
                "String connection for Dynamics", "s");

            CommandResourceName = new CommandParameterDefinition("resource",
                CommandParameterDefinition.TypeValue.String,
                "Resource name in CRM", "r");

            RegisterCommandParameter(CommandPathParameter);
            RegisterCommandParameter(CommandStringConnectionParameter);
            RegisterCommandParameter(CommandResourceName);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name)
                && IsParamOk(parameters, CommandStringConnectionParameter.Name)
                && IsParamOk(parameters, CommandResourceName.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var stringConnection = GetStringParameterValue(parameters, CommandStringConnectionParameter.Name);
            var resourcePath = GetStringParameterValue(parameters, CommandPathParameter.Name);
            var resourceName = GetStringParameterValue(parameters, CommandResourceName.Name);

            if (!FileService.ExistsFile(resourcePath))
            {
                throw new PathNotFoundException(resourcePath);
            }

            Log("Connecting to CRM...");
            IOrganizationService service = CrmProvider.GetService(stringConnection)
                ?? throw new Exception("Can't connect to CRM with given string connection");
            var completePath = FileService.GetAbsoluteCurrentPath(resourcePath);
            var data = FileService.ReadAllBytes(completePath);

            Log("Uploading webresource...");
            var id = CrmProvider.UploadWebResource(service, data, resourceName);
            Log("Publishing webresource...");
            CrmProvider.PublishWebResouce(service, id);
        }
    }
}
