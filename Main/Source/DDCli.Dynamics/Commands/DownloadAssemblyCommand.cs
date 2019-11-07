using DDCli.Dynamics.Utilities;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Commands
{
    public class DownloadAssemblyCommand : CommandBase
    {

        private const string HelpDefinition = "Download assembly registered in CRM";

        public CommandParameterDefinition CommandAssemblyIdParameter { get; set; }
        public CommandParameterDefinition CommandAssemblyNameParameter { get; set; }

        public CommandParameterDefinition CommandStringConnectionParameter { get; set; }
        public IFileService FileService { get; }

        public DownloadAssemblyCommand(IFileService fileService)
                : base(typeof(DownloadAssemblyCommand).Namespace, nameof(DownloadAssemblyCommand), HelpDefinition)
        {

            CommandStringConnectionParameter = new CommandParameterDefinition("stringconnection",
               CommandParameterDefinition.TypeValue.String,
               "String connection for Dynamics", "s");

            CommandAssemblyIdParameter = new CommandParameterDefinition("id",
                CommandParameterDefinition.TypeValue.Guid,
                "Assembly id in CRM", "i");

            CommandAssemblyNameParameter = new CommandParameterDefinition("name",
                CommandParameterDefinition.TypeValue.String,
                "Assembly name in CRM", "n");

            RegisterCommandParameter(CommandAssemblyIdParameter);
            RegisterCommandParameter(CommandStringConnectionParameter);
            RegisterCommandParameter(CommandAssemblyNameParameter);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }



        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandStringConnectionParameter.Name)
                && (IsParamOk(parameters, CommandAssemblyIdParameter.Name)
                        || IsParamOk(parameters, CommandAssemblyNameParameter.Name));


        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var stringConnection = GetStringParameterValue(parameters, CommandStringConnectionParameter.Name);
            var assemblyId = GetGuidParameterValue(parameters, CommandAssemblyIdParameter.Name);
            var assemblyName = GetStringParameterValue(parameters, CommandAssemblyNameParameter.Name);

            IOrganizationService service = CrmProvider.GetService(stringConnection);
            Guid id = Guid.Empty;
            if (assemblyId != Guid.Empty)
            {
                id = assemblyId;
            }
            else if (!string.IsNullOrEmpty(assemblyName))
            {
                QueryByAttribute qe = new QueryByAttribute("pluginassembly");
                qe.AddAttributeValue("name", assemblyName);
                var response = service.RetrieveMultiple(qe);
                if (response.Entities.Count == 0)
                {
                    throw new Exception($"Can't find any assembly with name '{assemblyName}'");
                }
                id = response.Entities[0].Id;
            }
            var assembly = service.Retrieve("pluginassembly", id, new ColumnSet("name", "content"));
            var path = $"{assembly["name"]}.dll";
            FileService.WriteAllBytes(path, Convert.FromBase64String((string)assembly["content"]));
        }
    }
}
