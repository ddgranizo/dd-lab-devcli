using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DDCli.Commands.DD
{
    public class PipelineCommand : CommandBase
    {

        public static string HelpDefinition { get; private set; } = "Execute multiples commands in one session";

        public CommandParameterDefinition CommandPathParameter { get; set; }
        public CommandParameterDefinition CommandNameParameter { get; set; }
        public List<CommandBase> RegisteredCommands { get; }
        public ILoggerService LoggerService { get; }
        public IFileService FileService { get; }
        public IRegistryService RegistryService { get; }
        public ICryptoService CryptoService { get; }
        public IStoredDataService StoredDataService { get; }

        public PipelineCommand(
            List<CommandBase> registeredCommands,
            ILoggerService loggerService,
            IFileService fileService,
            IRegistryService registryService,
            ICryptoService cryptoService,
            IStoredDataService storedDataService)
            : base(typeof(PipelineCommand).Namespace, nameof(PipelineCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition("path",
                CommandParameterDefinition.TypeValue.String,
                "Path with the ddpipeline.json file", "p");
            CommandNameParameter = new CommandParameterDefinition(
                "name",
                CommandParameterDefinition.TypeValue.String,
                "Name of the registered pipeline", "n");

            RegisterCommandParameter(CommandPathParameter);
            RegisterCommandParameter(CommandNameParameter);

            RegisteredCommands = registeredCommands ?? throw new ArgumentNullException(nameof(registeredCommands));
            LoggerService = loggerService ?? throw new ArgumentNullException(nameof(loggerService));
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            RegistryService = registryService ?? throw new ArgumentNullException(nameof(registryService));
            CryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
            StoredDataService = storedDataService ?? throw new ArgumentNullException(nameof(storedDataService));
        }


        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name)
                || IsParamOk(parameters, CommandNameParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            string path = string.Empty;
            if (IsParamOk(parameters, CommandNameParameter.Name))
            {
                var pipelineName = GetStringParameterValue(parameters, CommandNameParameter.Name);
                path = StoredDataService.GetPipelinePath(pipelineName);
            }
            else
            {
                path = GetStringParameterValue(parameters, CommandPathParameter.Name);
            }

            if (!FileService.ExistsFile(path))
            {
                throw new PathNotFoundException(path);
            }

            var pipelineConfig =
                ExceptionInLine.Run<DDPipelineConfig>(
                    () => { return FileService.GetPipelineConfig(path); },
                    (ex) => { throw new InvalidPipelineConfigFileException(ex.Message); });

            if (!FileService.IsValidPipelineConfiguration(pipelineConfig))
            {
                throw new InvalidPipelineConfigFileException();
            }

            var multipleCommandManager =
                new CommandManager(LoggerService ,StoredDataService, CryptoService, CommandManager.ExecutionModeTypes.Multiple);
            multipleCommandManager.RegisterCommands(RegisteredCommands);
            multipleCommandManager.OnLog += MultipleCommandManager_OnLog;
            multipleCommandManager.OnReplacedAutoIncrementInCommand += MultipleCommandManager_OnReplacedAutoIncrementInSubCommand;

            RequestInputVariables(pipelineConfig);

            Log($"## Executing pipeline {pipelineConfig.PipelineName}...");
            Stopwatch sb = new Stopwatch(); sb.Start();
            int counter = 0;
            int totalCommandsCound = pipelineConfig.Commands.Count;
            foreach (var commandDefinition in pipelineConfig.Commands)
            {
                var currentCounter = counter++;
                var commandName = string.IsNullOrEmpty(commandDefinition.Alias) ? "" : commandDefinition.Alias;
                if (commandDefinition.IsDisabled)
                {
                    Log($"### [{currentCounter}/{totalCommandsCound}] {commandName} | Skip command. Reason: Disabled");
                }
                else
                {
                    if (commandDefinition.IsIteration)
                    {
                        ExecuteIterationCommand(
                           multipleCommandManager,
                            commandDefinition.Command,
                            commandDefinition.ConsoleInputs,
                            pipelineConfig.PipelineConstants,
                            totalCommandsCound,
                            currentCounter,
                            commandName,
                            commandDefinition);
                    }
                    else
                    {
                        ExecuteCommand(
                            multipleCommandManager,
                            commandDefinition.Command,
                            commandDefinition.ConsoleInputs,
                            pipelineConfig.PipelineConstants,
                            totalCommandsCound,
                            currentCounter,
                            commandName);
                    }

                }
            }
            var time = StringFormats.MillisecondsToHumanTime(sb.ElapsedMilliseconds);
            Log($"## Completed pipeline {pipelineConfig.PipelineName} in {time}");
        }

        private void ExecuteIterationCommand(
            CommandManager multipleCommandManager,
            string command,
            List<string> consoleInputs,
            Dictionary<string, string> constants,
            int totalCommandCount,
            int currentCounter,
            string commandAlias,
            PipeLineCommandDefinition commandDefinition)
        {
           
            int iteration = 0;
            if (commandDefinition.IterationType == PipelineIterator.StringArray)
            {
                foreach (var item in commandDefinition.IterationTypeStringArrayValues)
                {
                    var replacedIterationCommand = command.Replace("[[i]]", item);
                    ExecuteCommand(
                        multipleCommandManager,
                        replacedIterationCommand,
                        consoleInputs,
                        constants,
                        totalCommandCount,
                        currentCounter,
                        commandAlias,
                        iteration);
                    iteration++;
                }
            }
            else if (commandDefinition.IterationType == PipelineIterator.Number)
            {
                for (iteration = commandDefinition.IterationTypeNumberFromValue; iteration < commandDefinition.IterationTypeNumberToValue; iteration++)
                {
                    var replacedIterationCommand = command.Replace("[[i]]", iteration.ToString());
                    ExecuteCommand(
                        multipleCommandManager,
                        replacedIterationCommand,
                        consoleInputs,
                        constants,
                        totalCommandCount,
                        currentCounter,
                        commandAlias,
                        iteration);
                }
            }
        }


        private void ExecuteCommand(
            CommandManager multipleCommandManager,
            string command,
            List<string> consoleInputs,
            Dictionary<string, string> constants,
            int totalCommandCount,
            int currentCounter,
            string commandAlias,
            int? iteration = null)
        {
            var replacedCommand = ReplaceConstants(constants, command);
            var args = GetCommandArgs(replacedCommand);
            if (iteration != null)
            {
                Log($"### [{currentCounter}/{totalCommandCount} i={iteration++}] {commandAlias} | Executing iteration command {replacedCommand}...");
            }
            else
            {
                Log($"### [{currentCounter}/{totalCommandCount}] {commandAlias} | Executing command {replacedCommand}...");
            }
            
            var inputCommand = new InputRequest(args);
            multipleCommandManager.ExecuteInputRequest(inputCommand, consoleInputs);
        }

        private static string[] GetCommandArgs(string replacedCommand)
        {
            var args = StringFormats.StringToParams(replacedCommand);
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                args[i] = arg.Replace("$$", "\"");
            }
            return args;
        }

        private void RequestInputVariables(DDPipelineConfig pipelineConfig)
        {
            if (pipelineConfig.PipelineVariables != null && pipelineConfig.PipelineVariables.Count > 0)
            {
                Log($"## Setting up pipeline variables:");
                foreach (var variable in pipelineConfig.PipelineVariables)
                {
                    Log($"\tValue for variable '{variable}':");
                    var value = Console.ReadLine();

                    pipelineConfig.PipelineConstants.Add(variable, value);
                }
            }
        }

        private static string ReplaceConstants(Dictionary<string, string> constants, string command)
        {
            var replaced = command;
            foreach (var key in constants.Keys)
            {
                replaced = replaced.Replace($"[[{key}]]", constants[key]);
            }
            return replaced;
        }

        private void MultipleCommandManager_OnReplacedAutoIncrementInSubCommand(object sender, Events.ReplacedParameterEventArgs args)
        {
            ParameterAutoIncrementeReplaced(args.Parameter);
        }

        private void MultipleCommandManager_OnLog(object sender, Events.LogEventArgs e)
        {
            Log(e.Log);
        }
    }
}
