using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.DotNet
{
    public class OpenVisualStudioCommand : CommandBase
    {
        private const string HelpDefinition = "open visual studio 2019 as administrator in current folder";

        public CommandParameterDefinition RootParameter { get; set; }
        public IPromptCommandService PromptCommandService { get; }
        public IFileService DirectoryService { get; }

        public OpenVisualStudioCommand(
            IPromptCommandService promptCommandService,
            IFileService directoryService)
            : base(typeof(OpenVisualStudioCommand).Namespace, nameof(OpenVisualStudioCommand), HelpDefinition)
        {
            RootParameter = new CommandParameterDefinition(
                "root",
                CommandParameterDefinition.TypeValue.Boolean,
                "Indicates if open the program as administrator");
            PromptCommandService = promptCommandService ?? throw new ArgumentNullException(nameof(promptCommandService));
            DirectoryService = directoryService ?? throw new ArgumentNullException(nameof(directoryService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }

        public override void Execute(List<CommandParameter> parameters)
        {

            //var fileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\devenv.exe";
            //bool runAsAdministrator = GetBoolParameterValue(parameters, RootParameter.Name, false);
            //PromptCommandService
            //    .Run(DirectoryService.GetCurrentPath(), fileName, DirectoryService.GetCurrentPath(), runAsAdministrator, true);
        }

        private void PromptCommandManager_OnCommandPromptOutput(string output)
        {
            Log(output);
        }
    }
}
