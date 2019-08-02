using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.Git
{
    public class CSharpGitIgnoreCommand : CommandBase
    {
        private const string GitIgnoreUrl = "https://raw.githubusercontent.com/ddgranizo/dd-lab-devcli/master/Public/Dev/Git/DefaultC%23Gitignore/.gitignore";
        private const string HelpDefinition = "Downloads the file stored in https://raw.githubusercontent.com/ddgranizo/dd-lab-devcli/master/Public/Dev/Git/DefaultC%23Gitignore/.gitignore in the current path";

        public IWebService WebService { get; }

        public CSharpGitIgnoreCommand(IWebService webService)
            : base(typeof(CSharpGitIgnoreCommand).Namespace, nameof(CSharpGitIgnoreCommand), HelpDefinition)
        {
            WebService = webService ?? throw new ArgumentNullException(nameof(webService));
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            if (!CheckAndExecuteHelpCommand(parameters))
            {
                WebService.DownloadFile(GitIgnoreUrl);
            }
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }
    }
}
