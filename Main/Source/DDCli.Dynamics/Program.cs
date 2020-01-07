using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Services;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics
{
    class Program
    {
        private static CommandManager commandManager;
        private static ILoggerService _loggerService;
        static void Main(string[] args)
        {
            //var argsV2 = StringFormats.StringToParams(string.Join(" ", args));
            _loggerService = new LoggerService();
            _loggerService.Log("###### INITIALIZED DYNAMICS CLI ######");
            LogRecievedArgs(args);
            bool isRecursive = args.Any(k => k.Length > 0 && k.First() == '\"');
            var argsV2 = isRecursive ? StringFormats.StringToParams(string.Join(" ", args)) : args;
            LogProcessedArgs(argsV2);

            var storedData = StoredDataManager.GetStoredData();

            _loggerService = new LoggerService();
            IRegistryService registryService = new RegistryService();
            ICryptoService cryptoService = new CryptoService(registryService);
            IStoredDataService storedDataService = new StoredDataService(storedData, cryptoService);

            commandManager = new CommandManager(_loggerService, storedDataService, cryptoService);
            commandManager.OnLog += CommandManager_OnLog;

            RegisterCommands(storedDataService, registryService, cryptoService);

            try
            {
                var inputCommand = new InputRequest(argsV2);
                commandManager.ExecuteInputRequest(inputCommand);
            }
            catch (PathNotFoundException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Path '{ex.Message}' does not exists");
            }
            catch (Exception ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Throwed uncatched exception: {ex.ToString()}");
            }
        }

        private static void CommandManager_OnLog(object sender, Events.LogEventArgs e)
        {
            _loggerService.Log(e.Log);
        }


        private static void LogProcessedArgs(string[] argsV2)
        {
            _loggerService.Log("ArgsV2:");
            foreach (var item in argsV2)
            {
                _loggerService.Log($"\t|{item}|");
            }
        }

        private static void LogRecievedArgs(string[] args)
        {
            _loggerService.Log("Retrieved args:");
            foreach (var item in args)
            {
                _loggerService.Log($"\t|{item}|");
            }
        }

        private static void RegisterCommands(
            IStoredDataService storedDataService,
            IRegistryService registryService,
            ICryptoService cryptoService)
        {
            IClipboardService clipboardService = new ClipboardService();
            IFileService fileService = new FileService();
            IPromptCommandService promptCommandService = new PromptCommandService();
            IWebService webService = new WebService();

            Register(new Commands.ImportSolutionCommand(fileService));
            Register(new Commands.GetTokenCommand());
            Register(new Commands.DownloadAssemblyCommand(fileService));
            Register(new Commands.HelpCommand(commandManager.Commands));
        }

        private static void Register(CommandBase command)
        {
            commandManager.RegisterCommand(command);
        }

    }
}
