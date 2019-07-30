using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Dev.Windows
{
    public class OpenRepoCommand : CommandBase
    {
        private const string SearchPath = @"C:\Users\daniel.diazg\MyRepos";
        private const string HelpDefinition = @"find and open repo in path C:\Users\daniel.diazg\MyRepos";


        public CommandParameterDefinition NameParameter { get; set; }

        public OpenRepoCommand()
            : base(typeof(OpenRepoCommand).Namespace, nameof(OpenRepoCommand), HelpDefinition)
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
                var name = GetStringParameterValue(parameters, NameParameter.Name);
                var directories = DirectoryUtilities.SearchDirectories(SearchPath, name, true);
                foreach (var item in directories)
                {
                    Log($"{directories.IndexOf(item) + 1} - {item}");
                };

                string indexString = Console.ReadLine();
                if (!int.TryParse(indexString, out int index))
                {
                    throw new InvalidCastException();
                }

                if (index < 1 || index > directories.Count)
                {
                    throw new IndexOutOfRangeException();
                }
                var path = directories[index - 1];
                Log($"1 - Open in explorer");
                Log($"2 - Open in new cmd");
                Log($"3 - Open in new conEmu");
                Log($"4 - Copy path to clipboard");

                indexString = Console.ReadLine();
                if (!int.TryParse(indexString, out index))
                {
                    throw new InvalidCastException();
                }

                if (index < 1 || index > 4)
                {
                    throw new IndexOutOfRangeException();
                }
                if (index == 1)
                {
                    PromptCommandManager.OpenExplorer(path);
                }
                else if (index == 2)
                {
                    PromptCommandManager.Run(path, @"cmd.exe", null, false, true);
                }
                else if (index == 3)
                {
                    PromptCommandManager.Run(path , @"C:\Program Files\ConEmu\ConEmu64.exe", null, false, true);
                }
                else if (index == 4)
                {
                    ClipboardManager.CopyToClipboard(path);
                }
            }
        }

        private void PromptCommandManager_OnCommandPromptOutput(string output)
        {
            Log(output);
        }
    }
}
