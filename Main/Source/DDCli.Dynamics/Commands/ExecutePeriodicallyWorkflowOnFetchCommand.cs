using DDCli.Dynamics.Utilities;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Commands
{
    public class ExecutePeriodicallyWorkflowOnFetchCommand : CommandBase
    {

        private const string HelpDefinition = "Execute workflow under fetch results periodically";

        public CommandParameterDefinition CommandAssemblyIdParameter { get; set; }
        public CommandParameterDefinition CommandAssemblyNameParameter { get; set; }
        public CommandParameterDefinition CommandStringConnectionParameter { get; set; }
        public CommandParameterDefinition FetchFilePathParameter { get; set; }
        public CommandParameterDefinition PeriodParameter { get; set; }

        public IFileService FileService { get; }

        public ExecutePeriodicallyWorkflowOnFetchCommand(IFileService fileService)
                : base(typeof(ExecutePeriodicallyWorkflowOnFetchCommand).Namespace, nameof(ExecutePeriodicallyWorkflowOnFetchCommand), HelpDefinition)
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

            FetchFilePathParameter = new CommandParameterDefinition("fetch",
                CommandParameterDefinition.TypeValue.String,
                "Path of file xml with the fetch inside", "f");

            PeriodParameter = new CommandParameterDefinition("period",
               CommandParameterDefinition.TypeValue.Integer,
               "Seconds for wait before execute a new fetch", "p");

            RegisterCommandParameter(CommandAssemblyIdParameter);
            RegisterCommandParameter(CommandStringConnectionParameter);
            RegisterCommandParameter(CommandAssemblyNameParameter);
            RegisterCommandParameter(FetchFilePathParameter);
            RegisterCommandParameter(PeriodParameter);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }



        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandStringConnectionParameter.Name)
                && (IsParamOk(parameters, CommandAssemblyIdParameter.Name)
                        || IsParamOk(parameters, CommandAssemblyNameParameter.Name))
                && IsParamOk(parameters, FetchFilePathParameter.Name)
                && IsParamOk(parameters, PeriodParameter.Name);

        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var stringConnection = GetStringParameterValue(parameters, CommandStringConnectionParameter.Name);
            var assemblyId = GetGuidParameterValue(parameters, CommandAssemblyIdParameter.Name);
            var assemblyName = GetStringParameterValue(parameters, CommandAssemblyNameParameter.Name);
            var fetchFile = GetStringParameterValue(parameters, FetchFilePathParameter.Name);
            var period = GetIntParameterValue(parameters, PeriodParameter.Name);

            Log("Retrieving organization service...");

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

            var existsFile = FileService.ExistsFile(fetchFile);
            if (!existsFile)
            {
                throw new Exception($"Can't find file with path '{fetchFile}'");
            }

            var fetch = FileService.ReadAllText(fetchFile);
            Log("Loaded fetch in file:");
            Log(fetch);

            while (true)
            {
                var records = CrmProvider.GetIdsFromFetch(service, fetch);
                Log($"Retrieved {records.Count} from fetch");
                foreach (var recordId in records)
                {
                    CrmProvider.ExecuteWorkflowOnRecord(service, id, recordId);
                    Log($"Executed workflow for recordId={recordId}");
                }
                Log($"Waiting {period} seconds...");
                System.Threading.Thread.Sleep(period * 1000);
            }

        }
    }
}
