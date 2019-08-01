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
        public OpenVisualStudioCommand()
            :base(typeof(OpenVisualStudioCommand).Namespace, nameof(OpenVisualStudioCommand), HelpDefinition)
        {
            RootParameter = new CommandParameterDefinition(
                "root",
                CommandParameterDefinition.TypeValue.Boolean,
                "Indicates if open the program as administrator");
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            if (!CheckAndExecuteHelpCommand(parameters))
            {
                var fileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\devenv.exe";
                bool runAsAdministrator = GetBoolParameterValue(parameters, RootParameter.Name, false);
                PromptCommandManager
                    .Run(DirectoryUtilities.GetCurrentPath(), fileName, DirectoryUtilities.GetCurrentPath(), runAsAdministrator, true);
            }
        }

        private void PromptCommandManager_OnCommandPromptOutput(string output)
        {
            Log(output);
        }
    }
}
