using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DDCli.Commands.DD
{
    public class PipelineCommand : CommandBase
    {

        public static string HelpDefinition { get; private set; } = "Execute multiples commands in one session";

        public CommandParameterDefinition CommandPathParameter { get; set; }
        public List<CommandBase> RegisteredCommands { get; }
        public IFileService FileService { get; }
        public IRegistryService RegistryService { get; }
        public ICryptoService CryptoService { get; }
        public IStoredDataService StoredDataService { get; }

        public PipelineCommand(
            List<CommandBase> registeredCommands, 
            IFileService fileService, 
            IRegistryService registryService,
            ICryptoService cryptoService,
            IStoredDataService storedDataService)
            : base(typeof(PipelineCommand).Namespace, nameof(PipelineCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition("path",
                CommandParameterDefinition.TypeValue.String,
                "Path with the ddpipeline.json file", "p");

            RegisterCommandParameter(CommandPathParameter);

            RegisteredCommands = registeredCommands ?? throw new ArgumentNullException(nameof(registeredCommands));
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            RegistryService = registryService ?? throw new ArgumentNullException(nameof(registryService));
            CryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
            StoredDataService = storedDataService ?? throw new ArgumentNullException(nameof(storedDataService));
        }


        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandPathParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var path = GetStringParameterValue(parameters, CommandPathParameter.Name);
            var directory = path;
            if (FileService.IsFile(path))
            {
                directory = FileService.GetFileDirectory(path);
            }
            if (!FileService.ExistsDirectory(directory))
            {
                throw new PathNotFoundException(directory);
            }
            if (!FileService.ExistsPipelineConfigFile(directory))
            {
                throw new PipelineConfigFileNotFoundException(directory);
            }

            var pipelineConfig =
                ExceptionInLine.Run<DDPipelineConfig>(
                    () => { return FileService.GetPipelineConfig(directory); },
                    (ex) => { throw new InvalidPipelineConfigFileException(ex.Message); });

            if (pipelineConfig == null)
            {
                throw new InvalidPipelineConfigFileException();
            }

            var multipleCommandManager =
                new CommandManager(StoredDataService, CryptoService, CommandManager.ExecutionModeTypes.Multiple);
            multipleCommandManager.RegisterCommands(RegisteredCommands);
            multipleCommandManager.OnLog += MultipleCommandManager_OnLog;
            multipleCommandManager.OnReplacedAutoIncrementInCommand += MultipleCommandManager_OnReplacedAutoIncrementInSubCommand;
            foreach (var commandDefinition in pipelineConfig.Commands)
            {
                var args = StringFormats.StringToParams(commandDefinition.Command);
                var inputCommand = new InputRequest(args);
                multipleCommandManager.ExecuteInputRequest(inputCommand, commandDefinition.ConsoleInputs);
            }
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
