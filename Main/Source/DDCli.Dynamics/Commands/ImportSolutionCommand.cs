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
    public class ImportSolutionCommand : CommandBase
    {

        private const string HelpDefinition = "Imports solution to Dynamics instance";

        public CommandParameterDefinition CommandPathParameter { get; set; }

        public CommandParameterDefinition CommandStringConnectionParameter { get; set; }

        public CommandParameterDefinition CommandOverwriteUnmanagedCustomizationsParameter { get; set; }

        public CommandParameterDefinition CommandMigrateAsHoldParameter { get; set; }

        public CommandParameterDefinition CommandPublishWorkflowsParameter { get; set; }

        public CommandParameterDefinition ImportAsyncParameter { get; set; }
        public IFileService FileService { get; }

        public ImportSolutionCommand(IFileService fileService)
                : base(typeof(ImportSolutionCommand).Namespace, nameof(ImportSolutionCommand), HelpDefinition)
        {

            CommandPathParameter = new CommandParameterDefinition("path",
                CommandParameterDefinition.TypeValue.String,
                "Path with the zip solution", "p");

            CommandStringConnectionParameter = new CommandParameterDefinition("stringconnection",
                CommandParameterDefinition.TypeValue.String,
                "String connection for Dynamics", "s");

            CommandOverwriteUnmanagedCustomizationsParameter = new CommandParameterDefinition("overwriteunmanagedcustomizations",
                CommandParameterDefinition.TypeValue.Boolean,
                "Overwrite unmanaged customizations", "ov");

            CommandMigrateAsHoldParameter = new CommandParameterDefinition("migrateashold",
                CommandParameterDefinition.TypeValue.Boolean,
                "Migrate solution as hold", "mh");

            CommandPublishWorkflowsParameter = new CommandParameterDefinition("publishworkflows",
                CommandParameterDefinition.TypeValue.Boolean,
                "Publish workflows", "pw");

            ImportAsyncParameter = new CommandParameterDefinition("async",
               CommandParameterDefinition.TypeValue.Boolean,
               "Import solution async", "a");

        

            RegisterCommandParameter(CommandPathParameter);
            RegisterCommandParameter(CommandStringConnectionParameter);
            RegisterCommandParameter(CommandOverwriteUnmanagedCustomizationsParameter);
            RegisterCommandParameter(CommandMigrateAsHoldParameter);
            RegisterCommandParameter(CommandPublishWorkflowsParameter);
            RegisterCommandParameter(ImportAsyncParameter);

            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        }

        

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name)
                && IsParamOk(parameters, CommandStringConnectionParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var stringConnection = GetStringParameterValue(parameters, CommandStringConnectionParameter.Name);
            var solutionPath = GetStringParameterValue(parameters, CommandPathParameter.Name);
            
            if (!FileService.ExistsFile(solutionPath))
            {
                throw new PathNotFoundException(solutionPath);
            }

            IOrganizationService service = CrmProvider.GetService(stringConnection);
            var completePath = FileService.GetAbsoluteCurrentPath(solutionPath);
            var data = FileService.ReadAllBytes(completePath);

            bool migrateAsHold = GetBoolParameterValue(parameters, CommandMigrateAsHoldParameter.Name, false);
            bool overwriteUnmanagedCustomizations = GetBoolParameterValue(parameters, CommandOverwriteUnmanagedCustomizationsParameter.Name, true);
            bool publishWorkflows = GetBoolParameterValue(parameters, CommandPublishWorkflowsParameter.Name, true);
            bool async = GetBoolParameterValue(parameters, ImportAsyncParameter.Name, false);

            if (async)
            {
                CrmProvider.ImportSolutionsAsync(service, data, overwriteUnmanagedCustomizations, migrateAsHold, publishWorkflows);
            }
            else
            {
                CrmProvider.ImportSolutions(service, data, overwriteUnmanagedCustomizations, migrateAsHold, publishWorkflows);
            }

        }
    }
}
