using DDCli.Exceptions;
using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DDCli.Commands.Dev.Utils
{
    public class TemplateCommand : CommandBase
    {
        private const string HelpDefinition = @"clone folder using ddtemplate.json configuration";

        private Dictionary<string, string> _userInputs = new Dictionary<string, string>();

        public CommandParameterDefinition CommandPathParameter { get; set; }
        public CommandParameterDefinition CommandNameParameter { get; set; }
        public IFileService FileService { get; }
        public IStoredDataService StoredDataService { get; }
        public List<ReplacePairValue> UserTemplateSetupReplaceStrings { get; set; }
        public TemplateCommand(
            IFileService directoryService, 
            IStoredDataService storedDataService)
             : base(typeof(TemplateCommand).Namespace, nameof(TemplateCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition(
                "path",
                CommandParameterDefinition.TypeValue.String,
                "path for clone with ddtemplate.json",
                "p");

            CommandNameParameter = new CommandParameterDefinition(
                "name",
                CommandParameterDefinition.TypeValue.String,
                "Name of the registered template",
                "n");

            FileService = directoryService
                ?? throw new ArgumentNullException(nameof(directoryService));
            StoredDataService = storedDataService ?? throw new ArgumentNullException(nameof(storedDataService));
            RegisterCommandParameter(CommandPathParameter);
            RegisterCommandParameter(CommandNameParameter);
            UserTemplateSetupReplaceStrings = new List<ReplacePairValue>();
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return (IsParamOk(parameters, CommandPathParameter.Name)
                    || IsParamOk(parameters, CommandNameParameter.Name))
                    && !(IsParamOk(parameters, CommandPathParameter.Name) 
                            && IsParamOk(parameters, CommandNameParameter.Name));
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            string path = string.Empty;
            if (IsParamOk(parameters, CommandNameParameter.Name))
            {
                var templateName = GetStringParameterValue(parameters, CommandNameParameter.Name);
                path = StoredDataService.GetTemplatePath(templateName);
            }
            else
            {
                path = GetStringParameterValue(parameters, CommandPathParameter.Name);
            }
            if (!FileService.ExistsDirectory(path))
            {
                throw new PathNotFoundException(path);
            }
            if (!FileService.ExistsTemplateConfigFile(path))
            {
                throw new TemplateConfigFileNotFoundException(path);
            }

            var templateConfig =
                ExceptionInLine.Run<DDTemplateConfig>(
                    () => { return FileService.GetTemplateConfig(path); },
                    (ex) => { throw new InvalidTemplateConfigFileException(ex.Message); });

            if (templateConfig == null)
            {
                throw new InvalidTemplateConfigFileException();
            }


            Log($"Set up for template '{templateConfig.TemplateName}'");
            Log($"Type destination folder:");
            var destinationPath = ConsoleService.ReadLine();
            
            Log($"Complete the paris for replace in the base project");
            foreach (var pair in templateConfig.ReplacePairs)
            {
                Log($"\t{pair.ReplaceDescription}: (Old value = {pair.OldValue})");
                var value = ConsoleService.ReadLine();
                var replacedPair = new ReplacePairValue(pair, value);
                UserTemplateSetupReplaceStrings.Add(replacedPair);
            }

            var absoluteDestionPath = FileService.GetAbsoluteCurrentPath(destinationPath);

            Log($"The template will be cloned at '{absoluteDestionPath}'");
            FileService.CreateDirectory(absoluteDestionPath, true);
            Log($"Cloning files...");
            var clonedFiles = FileService.CloneDirectory(path, absoluteDestionPath, templateConfig.IgnorePathPatterns);
            Log(clonedFiles.ToDisplayList($"Cloned {clonedFiles.Count} files:", "", false));

            foreach (var replaceString in UserTemplateSetupReplaceStrings)
            {
                Log($"Applying replacemene of string '{replaceString.ReplacedPair.OldValue}'->'{replaceString.Value}'. Apply for directories:{replaceString.ReplacedPair.ApplyForDirectories}, Apply for file names:{replaceString.ReplacedPair.ApplyForFileNames}, Apply for directories:{replaceString.ReplacedPair.ApplyForFileContents}");
                FileService.ReplaceStringInPaths(
                    absoluteDestionPath,
                    replaceString.ReplacedPair.OldValue,
                    replaceString.Value,
                    replaceString.ReplacedPair.ApplyForDirectories,
                    replaceString.ReplacedPair.ApplyForFileNames,
                    replaceString.ReplacedPair.ApplyForFileContents,
                    replaceString.ReplacedPair.ApplyForFilePattern);
            }

        }
    }
}
