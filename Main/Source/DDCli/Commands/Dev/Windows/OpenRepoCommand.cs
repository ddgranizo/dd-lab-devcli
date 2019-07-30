using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.Windows
{
    public class OpenRepoCommand : CommandBase
    {
        private const string HelpDefinition = @"find and open repo in path C:\Users\daniel.diazg\MyRepos";


        public CommandParameterDefinition NameParameter { get; set; }

        public OpenRepoCommand()
            :base(typeof(OpenRepoCommand).Namespace, nameof(OpenRepoCommand), HelpDefinition)
        {
            NameParameter = new CommandParameterDefinition(
                "name",
                CommandParameterDefinition.TypeValue.String,
                "Indicates the name or part of it for search and open this folder");

            CommandParametersDefinition.Add(NameParameter);
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, NameParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            if (!CheckAndExecuteHelpCommand(parameters))
            {
                PromptCommandManager manager = new PromptCommandManager();
                var fileName = @"C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\Common7\IDE\devenv.exe";
                manager.RunAs(PathUtilities.GetCurrentPath(), fileName, PathUtilities.GetCurrentPath());
            }
        }

        private void PromptCommandManager_OnCommandPromptOutput(string output)
        {
            Log(output);
        }
    }
}
