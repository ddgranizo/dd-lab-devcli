using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.Git
{
    public class CSharpGitIgnoreCommand : CommandBase
    {

        private const string HelpDefinition = "Downloads the file stored in ";


        public CSharpGitIgnoreCommand()
            :base(nameof(CSharpGitIgnoreCommand), typeof(CSharpGitIgnoreCommand).Namespace, HelpDefinition)
        {
        }
    }
}
