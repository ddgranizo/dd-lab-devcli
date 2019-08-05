using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.DotNet
{
    public class PublishDebugWinCommand : CommandBase
    {

        private const string HelpDefinition = "execute 'dotnet publish -c Debug -r win10-x64' command for either solutions or projects";


        public PublishDebugWinCommand()
            :base(typeof(PublishDebugWinCommand).Namespace, nameof(PublishDebugWinCommand), HelpDefinition)
        {

        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            
                //PromptCommandManager manager = new PromptCommandManager();
                //manager.Run("dotnet publish -c Debug -r win10-x64",);
            
        }

        private void PromptCommandManager_OnCommandPromptOutput(string output)
        {
            Log(output);
        }
    }
}
