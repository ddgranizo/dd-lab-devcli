using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Services;
using DDCli.Utilities;
using System;

namespace DDCli
{
    class Program
    {

        private static CommandManager commandManager;
        static void Main(string[] args)
        {
            commandManager = new CommandManager();
            commandManager.OnLog += CommandManager_OnLog;

            var storedData = StoredDataManager.GetStoredData();

            RegisterCommands(storedData);
            try
            {
                var inputCommand = new InputRequest(args);
                commandManager.ExecuteInputRequest(inputCommand);
            }
            catch (DuplicateCommandException ex)
            {
                Console.WriteLine($"Found {ex.Commands.Count} commands with the same name in different namespaces. In this case is necessary use the namespace for execute it. Commands: {string.Join(",", ex.Commands.ToArray())}");
            }
            catch (CommandNotFoundException)
            {
                Console.WriteLine($"Command not found. Use 'help' for check the available commands");
            }
            catch (InvalidParamsException)
            {
                Console.WriteLine($"This command cannot be executed with this combination of parameters");
            }
            catch (InvalidParamNameException ex)
            {
                Console.WriteLine($"Invalid parameter name '{ex.Message}'");
            }
            catch (NotArgumentsException)
            {
                Console.WriteLine($"Check all the params with 'help' command");
            }
            catch (NotValidCommandNameException ex)
            {
                Console.WriteLine($"Invalid command name '{ex.Message}'");
            }
            catch (AliasRepeatedException ex)
            {
                Console.WriteLine($"Alias '{ex.Message}' is already used.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Throwed uncatched exception: {ex.ToString()}");
            }

        }

        private static void CommandManager_OnLog(object sender, Events.LogEventArgs e)
        {
            Console.WriteLine(e.Log);
        }

        private static void RegisterCommands(StoredCliData storedData)
        {
            IClipboardService clipboardService = new ClipboardService();
            IDirectoryService directoryService = new DirectoryService();
            IPromptCommandService promptCommandService = new PromptCommandService();
            IWebService webService = new WebService();
            IStoredDataService storedDataService = new StoredDataService(storedData);

            Register(new Commands.Dev.Git.CSharpGitIgnoreCommand(webService));
            Register(new Commands.Dev.DotNet.PublishDebugWinCommand());
            Register(new Commands.Dev.DotNet.PublishReleaseWinCommand());
            Register(new Commands.Dev.DotNet.OpenVisualStudioCommand(promptCommandService, directoryService));
            Register(new Commands.Dev.Windows.OpenRepoCommand(directoryService, promptCommandService, clipboardService));
            Register(new Commands.Dev.Utils.SetAlias());


            //Last commands for register
            Register(new Commands.DD.AddAliasCommand(storedDataService, commandManager.Commands));
        }

        private static void Register(CommandBase command)
        {
            commandManager.RegisterCommand(command);
        }
    }

}
