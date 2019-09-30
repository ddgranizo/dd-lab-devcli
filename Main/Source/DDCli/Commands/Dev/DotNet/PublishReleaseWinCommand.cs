using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.DotNet
{
    public class PublishReleaseWinCommand : CommandBase
    {

        private const string HelpDefinition = "execute 'dotnet publish -c Release -r win10-x64' command for either solutions or projects";


        public PublishReleaseWinCommand()
            : base(typeof(PublishReleaseWinCommand).Namespace, nameof(PublishReleaseWinCommand), HelpDefinition)
        {

        }


        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }

        public override void Execute(List<CommandParameter> parameters)
        {
           
                //PromptCommandManager manager = new PromptCommandManager();
                //PromptCommandManager.OnCommandPromptOutput += PromptCommandManager_OnCommandPromptOutput;
                //manager.Run("dotnet publish -c Release -r win10-x64");
        }

    }
}
