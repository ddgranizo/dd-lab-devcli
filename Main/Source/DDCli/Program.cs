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

            RegisterCommands(storedDataService, registryService, cryptoService);

            try
            {
                var inputCommand = new InputRequest(args);
                commandManager.ExecuteInputRequest(inputCommand);
            }
            catch (DuplicateCommandException ex)
            {
                ExceptionManager.RaiseException($"Found {ex.Commands.Count} commands with the same name in different namespaces. In this case is necessary use the namespace for execute it. Commands: {string.Join(",", ex.Commands.ToArray())}");
            }
            catch (CommandNotFoundException)
            {
                ExceptionManager.RaiseException($"Command not found. Use 'help' for check the available commands");
            }
            catch (InvalidParamsException)
            {
                ExceptionManager.RaiseException($"This command cannot be executed with this combination of parameters");
            }
            catch (InvalidParamNameException ex)
            {
                ExceptionManager.RaiseException($"Invalid parameter name '{ex.Message}'");
            }
            catch (NotArgumentsException)
            {
                ExceptionManager.RaiseException($"Check all the params with 'help' command");
            }
            catch (NotValidCommandNameException ex)
            {
                ExceptionManager.RaiseException($"Invalid command name '{ex.Message}'");
            }
            catch (AliasRepeatedException ex)
            {
                ExceptionManager.RaiseException($"Alias '{ex.Message}' is already used");
            }
            catch (AliasNotFoundException ex)
            {
                ExceptionManager.RaiseException($"Alias '{ex.Message}' is not registered");
            }
            catch (ParameterRepeatedException ex)
            {
                ExceptionManager.RaiseException($"Parameter '{ex.Message}' is already used");
            }
            catch (ParameterNotFoundException ex)
            {
                ExceptionManager.RaiseException($"Parameter '{ex.Message}' is not registered");
            }
            catch (InvalidParamException ex)
            {
                ExceptionManager.RaiseException($"Cannot resolver parameter {ex.Message}");
            }
            catch (PathNotFoundException ex)
            {
                ExceptionManager.RaiseException($"Path '{ex.Message}' does not exists");
            }
            catch (TemplateConfigFileNotFoundException)
            {
                ExceptionManager.RaiseException($"Can't find '{Definitions.TemplateConfigFilename}' file in path");
            }
            catch (InvalidTemplateConfigFileException ex)
            {
                ExceptionManager.RaiseException($"Config file '{Definitions.TemplateConfigFilename}' is invalid. Error parsing: {ex.Message}");
            }
            catch (InvalidStringFormatException ex)
            {
                ExceptionManager.RaiseException($"Invalid string format. {ex.Message}");
            }
            catch (TemplateNameRepeatedException)
            {
                ExceptionManager.RaiseException($"Template name repeated");
            }
            catch (TemplateNotFoundException)
            {
                ExceptionManager.RaiseException($"Can't find any template with this name");
            }
            catch (RepositoryNotFoundException)
            {
                ExceptionManager.RaiseException($"Can't find any repository with this name");
            }
            catch (PipelineConfigFileNotFoundException)
            {
                ExceptionManager.RaiseException($"Can't find '{Definitions.PipelineConfigFilename}' file in path");
            }
            catch (InvalidPipelineConfigFileException ex)
            {
                ExceptionManager.RaiseException($"Config file '{Definitions.PipelineConfigFilename}' is invalid. Error parsing: {ex.Message}");
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


            Register(new Commands.DD.EchoCommand());
            Register(new Commands.DD.WindowsCmdCommand(promptCommandService));
            Register(new Commands.Dev.Git.CSharpGitIgnoreCommand(webService));
            Register(new Commands.Dev.DotNet.PublishDebugWinCommand());
            Register(new Commands.Dev.DotNet.PublishReleaseWinCommand());
            Register(new Commands.Dev.DotNet.OpenVisualStudioCommand(promptCommandService, fileService));
            Register(new Commands.Windows.OpenRepoCommand(fileService, promptCommandService, clipboardService));
            Register(new Commands.DD.AddParameterCommand(storedDataService));
            Register(new Commands.DD.ShowParametersCommand(storedDataService));
            Register(new Commands.DD.DeleteParameterCommand(storedDataService));
            Register(new Commands.DD.UpdateParameterCommand(storedDataService));
            Register(new Commands.DD.DeleteAliasCommand(storedDataService));
            Register(new Commands.DD.ShowAliasCommand(storedDataService));
            Register(new Commands.Dev.Utils.TemplateCommand(fileService, storedDataService));
            Register(new Commands.DD.AddTemplateCommand(storedDataService, fileService));
            Register(new Commands.DD.DeleteTemplateCommand(storedDataService));
            Register(new Commands.DD.ShowTemplatesCommand(storedDataService));
            Register(new Commands.Windows.ZipCommand(fileService));
            Register(new Commands.Windows.UnzipCommand(fileService));
            Register(new Commands.Windows.ReplaceFileContentCommand(fileService));
            Register(new Commands.Windows.RenameFolderCommand(fileService));

            //Last commands for register
            Register(new Commands.DD.AddAliasCommand(storedDataService, commandManager.Commands));

            Register(new Commands.DD.PipelineCommand(commandManager.Commands, fileService, registryService, cryptoService, storedDataService));
            Register(new Commands.HelpCommand(commandManager.Commands));
        }

        private static void Register(CommandBase command)
        {
            commandManager.RegisterCommand(command);
        }
    }

}
