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
            var storedData = StoredDataManager.GetStoredData();

            IRegistryService registryService = new RegistryService();
            ICryptoService cryptoService = new CryptoService(registryService);
            IStoredDataService storedDataService = new StoredDataService(storedData, cryptoService);

            commandManager = new CommandManager(storedDataService, cryptoService);
            commandManager.OnLog += CommandManager_OnLog;

            RegisterCommands(storedData, storedDataService);

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
                Console.WriteLine($"Alias '{ex.Message}' is already used");
            }
            catch (AliasNotFoundException ex)
            {
                Console.WriteLine($"Alias '{ex.Message}' is not registered");
            }
            catch (ParameterRepeatedException ex)
            {
                Console.WriteLine($"Parameter '{ex.Message}' is already used");
            }
            catch (ParameterNotFoundException ex)
            {
                Console.WriteLine($"Parameter '{ex.Message}' is not registered");
            }
            catch (InvalidParamException ex)
            {
                Console.WriteLine($"Cannot resolver parameter {ex.Message}");
            }
            catch (PathNotFoundException ex)
            {
                Console.WriteLine($"Path '{ex.Message}' does not exists");
            }
            catch (TemplateConfigFileNotFoundException ex)
            {
                Console.WriteLine($"Can't find '{Definitions.TemplateConfigFilename}' file in '{ex.Message}' path");
            }
            catch (InvalidTemplateConfigFileException ex)
            {
                Console.WriteLine($"Config file '{Definitions.TemplateConfigFilename}' is invalid. Error parsing: {ex.Message}");
            }
            catch (InvalidStringFormatException ex)
            {
                Console.WriteLine($"Invalid string format. {ex.Message}");
            }
            catch (TemplateNameRepeatedException)
            {
                Console.WriteLine($"Template name repeated");
            }
            catch (TemplateNotFoundException)
            {
                Console.WriteLine($"Can't find any template with this name");
            }
            catch (RepositoryNotFoundException)
            {
                Console.WriteLine($"Can't find any repository with this name");
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

        private static void RegisterCommands(StoredCliData storedData, IStoredDataService storedDataService)
        {
            IClipboardService clipboardService = new ClipboardService();
            IFileService fileService = new FileService();
            IPromptCommandService promptCommandService = new PromptCommandService();
            IWebService webService = new WebService();
            IConsoleService consoleService = new ConsoleService();

            Register(new Commands.Dev.Git.CSharpGitIgnoreCommand(webService));
            Register(new Commands.Dev.DotNet.PublishDebugWinCommand());
            Register(new Commands.Dev.DotNet.PublishReleaseWinCommand());
            Register(new Commands.Dev.DotNet.OpenVisualStudioCommand(promptCommandService, fileService));
            Register(new Commands.Dev.Windows.OpenRepoCommand(fileService, promptCommandService, clipboardService));
            Register(new Commands.DD.AddParameterCommand(storedDataService));
            Register(new Commands.DD.ShowParametersCommand(storedDataService));
            Register(new Commands.DD.DeleteParameterCommand(storedDataService));
            Register(new Commands.DD.UpdateParameterCommand(storedDataService));
            Register(new Commands.DD.DeleteAliasCommand(storedDataService));
            Register(new Commands.DD.ShowAliasCommand(storedDataService));
            Register(new Commands.Dev.Utils.TemplateCommand(fileService, consoleService, storedDataService));
            Register(new Commands.DD.AddTemplateCommand(storedDataService, fileService));
            Register(new Commands.DD.DeleteTemplateCommand(storedDataService));
            Register(new Commands.DD.ShowTemplatesCommand(storedDataService));

            //Last commands for register
            Register(new Commands.DD.AddAliasCommand(storedDataService, commandManager.Commands));

            Register(new Commands.HelpCommand(commandManager.Commands));
        }

        private static void Register(CommandBase command)
        {
            commandManager.RegisterCommand(command);
        }
    }

}
