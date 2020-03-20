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


        public CommandParameterDefinition CommandPathParameter { get; set; }
        public CommandParameterDefinition CommandNameParameter { get; set; }
        public CommandParameterDefinition DestinationPathParameter { get; set; }
        public CommandParameterDefinition ValuesParameter { get; set; }

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
                "Path for clone with ddtemplate.json",
                "p");

            CommandNameParameter = new CommandParameterDefinition(
                "name",
                CommandParameterDefinition.TypeValue.String,
                "Name of the registered template",
                "n");

            DestinationPathParameter = new CommandParameterDefinition(
               "destinationpath",
               CommandParameterDefinition.TypeValue.String,
               "Destination path for place the cloned template",
               "d");

            ValuesParameter = new CommandParameterDefinition(
              "values",
              CommandParameterDefinition.TypeValue.String,
              "Values for replace in template. Use pairs like \"Key1=Value 1;Key2=Value 2;...\"",
              "v");
           

            FileService = directoryService
                ?? throw new ArgumentNullException(nameof(directoryService));
            StoredDataService = storedDataService ?? throw new ArgumentNullException(nameof(storedDataService));
            RegisterCommandParameter(CommandPathParameter);
            RegisterCommandParameter(CommandNameParameter);
            RegisterCommandParameter(DestinationPathParameter);
            RegisterCommandParameter(ValuesParameter);

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
                var regardingPath = GetStringParameterValue(parameters, CommandPathParameter.Name);
                if (FileService.IsFile(regardingPath))
                {
                    path = FileService.GetFilePath(regardingPath);
                }
                else
                {
                    path = regardingPath;
                }
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


            var destinationPathRequest = GetStringParameterValue(parameters, DestinationPathParameter.Name);
            var destinationPath = string.Empty;
            if (string.IsNullOrEmpty(destinationPathRequest))
            {
                Log($"Set up for template '{templateConfig.TemplateName}'");
                Log($"Type destination folder:");
                destinationPath = ConsoleService.ReadLine();
            }
            else
            {
                destinationPath = destinationPathRequest;
            }
            var valuesRequest = GetStringParameterValue(parameters, ValuesParameter.Name);
            if (string.IsNullOrEmpty(valuesRequest))
            {
                Log($"Complete the paris for replace in the base project");
                foreach (var pair in templateConfig.ReplacePairs)
                {
                    Log($"\t{pair.ReplaceDescription}: (Old value = {pair.OldValue})");
                    var value = ConsoleService.ReadLine();
                    var replacedPair = new ReplacePairValue(pair, value);
                    UserTemplateSetupReplaceStrings.Add(replacedPair);
                }
            }
            else
            {
                UserTemplateSetupReplaceStrings.AddRange(valuesRequest.Split(';').Select(k =>
                {
                    if (k.IndexOf("=") == -1)
                    {
                        throw new Exception("Invalid pair param=value");
                    }
                    var keyValue = k.Split('=');
                    var key = keyValue[0];
                    var value = keyValue[1];
                    if (string.IsNullOrEmpty(key)
                            || string.IsNullOrEmpty(value))
                    {
                        throw new Exception("Invalid pair param=value");
                    }
                    var pair = templateConfig.ReplacePairs.FirstOrDefault(l => l.OldValue == key);
                    if (pair == null)
                    {
                        throw new Exception($"Param '{key}' is not defined in the template.json");
                    }
                    return new ReplacePairValue(pair, value);
                }));
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

                var oldValue = replaceString.ReplacedPair.OldValue;
                var newValue = replaceString.Value;
                var rootPath = absoluteDestionPath;
                var pattern = replaceString.ReplacedPair.ApplyForFilePattern;
                if (replaceString.ReplacedPair.ApplyForDirectories)
                {
                    FileService.ReplaceAllSubDirectoriesName(rootPath, oldValue, newValue);
                }
                if (replaceString.ReplacedPair.ApplyForFileNames)
                {
                    FileService.ReplaceAllFilesName(rootPath, oldValue, newValue);
                }
                if (replaceString.ReplacedPair.ApplyForFileContents)
                {
                    FileService.ReplaceFilesContents(rootPath, oldValue, newValue, pattern);
                }
            }
        }
    }
}
