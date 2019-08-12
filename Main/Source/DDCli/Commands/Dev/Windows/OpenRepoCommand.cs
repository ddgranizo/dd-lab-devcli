using DDCli.Interfaces;
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
        public IFileService DirectoryService { get; }
        public IPromptCommandService PromptCommandService { get; }
        public IClipboardService ClipboardService { get; }

        public OpenRepoCommand(
            IFileService directoryService,
            IPromptCommandService promptCommandService,
            IClipboardService clipboardService)
            : base(typeof(OpenRepoCommand).Namespace, nameof(OpenRepoCommand), HelpDefinition)
        {
            NameParameter = new CommandParameterDefinition(
                "name",
                CommandParameterDefinition.TypeValue.String,
                "Indicates the name or part of it for search and open this folder",
                "n");

            CommandParametersDefinition.Add(NameParameter);
            DirectoryService = directoryService ?? throw new ArgumentNullException(nameof(directoryService));
            PromptCommandService = promptCommandService ?? throw new ArgumentNullException(nameof(promptCommandService));
            ClipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, NameParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {

            var name = GetStringParameterValue(parameters, NameParameter.Name);
            var directories = DirectoryService.SearchDirectories(SearchPath, name, true);
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
            Log($"5 - Open origin");

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
                PromptCommandService.OpenExplorer(path);
            }
            else if (index == 2)
            {
                PromptCommandService.Run(path, @"cmd.exe", null, false, true);
            }
            else if (index == 3)
            {
                PromptCommandService.Run(path, @"C:\Program Files\ConEmu\ConEmu64.exe", null, false, true);
            }
            else if (index == 4)
            {
                ClipboardService.CopyToClipboard(path);
            }
        }

        private void PromptCommandManager_OnCommandPromptOutput(string output)
        {
            Log(output);
        }
    }
}
