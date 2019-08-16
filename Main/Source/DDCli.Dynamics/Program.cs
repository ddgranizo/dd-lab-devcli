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
        static void Main(string[] args)
        {
            var storedData = StoredDataManager.GetStoredData();

            IRegistryService registryService = new RegistryService();
            ICryptoService cryptoService = new CryptoService(registryService);
            IStoredDataService storedDataService = new StoredDataService(storedData, cryptoService);

            commandManager = new CommandManager(storedDataService, cryptoService);
            commandManager.OnLog += CommandManager_OnLog;

            RegisterCommands(storedDataService, registryService, cryptoService);

            try
            {
                var inputCommand = new InputRequest(args);
                commandManager.ExecuteInputRequest(inputCommand);
            }
            catch(PathNotFoundException ex)
            {
                ExceptionManager.RaiseException($"Path '{ex.Message}' does not exists");
            }
            catch (Exception ex)
            {
                ExceptionManager.RaiseException($"Throwed uncatched exception: {ex.ToString()}");
            }
        }

        private static void CommandManager_OnLog(object sender, Events.LogEventArgs e)
        {
            Console.WriteLine(e.Log);
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
            Register(new Commands.HelpCommand(commandManager.Commands));
        }

        private static void Register(CommandBase command)
        {
            commandManager.RegisterCommand(command);
        }

    }
}
