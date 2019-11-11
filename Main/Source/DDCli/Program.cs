using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Services;
using DDCli.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace DDCli
{
    class Program
    {
        
        private static CommandManager commandManager;
        private static ILoggerService _loggerService;
        static void Main(string[] args)
        {
            _loggerService = new LoggerService();
            _loggerService.Log("###### INITIALIZED CLI ######");
            LogRecievedArgs(args);
            var argsV2 = StringFormats.StringToParams(string.Join(" ", args.Select(k => $"\"{k}\"")));
            LogProcessedArgs(argsV2);
            var storedData = StoredDataManager.GetStoredData();

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
            catch (DuplicateCommandException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Found {ex.Commands.Count} commands with the same name in different namespaces. In this case is necessary use the namespace for execute it. Commands: {string.Join(",", ex.Commands.ToArray())}");
            }
            catch (CommandNotFoundException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Command not found. Use 'help' for check the available commands");
            }
            catch (InvalidParamsException)
            {
                ExceptionManager.RaiseException(_loggerService, $"This command cannot be executed with this combination of parameters");
            }
            catch (InvalidParamNameException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Invalid parameter name '{ex.Message}'");
            }
            catch (NotArgumentsException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Check all the params with 'help' command");
            }
            catch (NotValidCommandNameException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Invalid command name '{ex.Message}'");
            }
            catch (AliasRepeatedException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Alias '{ex.Message}' is already used");
            }
            catch (AliasNotFoundException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Alias '{ex.Message}' is not registered");
            }
            catch (ParameterRepeatedException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Parameter '{ex.Message}' is already used");
            }
            catch (ParameterNotFoundException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Parameter '{ex.Message}' is not registered");
            }
            catch (InvalidParamException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Cannot resolver parameter {ex.Message}");
            }
            catch (PathNotFoundException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Path '{ex.Message}' does not exists");
            }
            catch (TemplateConfigFileNotFoundException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Can't find '{Definitions.TemplateConfigFilename}' file in path");
            }
            catch (InvalidTemplateConfigFileException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Config file '{Definitions.TemplateConfigFilename}' is invalid. Error parsing: {ex.Message}");
            }
            catch (InvalidStringFormatException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Invalid string format. {ex.Message}");
            }
            catch (TemplateNameRepeatedException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Template name repeated");
            }
            catch (TemplateNotFoundException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Can't find any template with this name");
            }
            catch (RepositoryNotFoundException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Can't find any repository with this name");
            }
            catch (PipelineConfigFileNotFoundException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Can't find '{Definitions.PipelineConfigFilename}' file in path");
            }
            catch (InvalidPipelineConfigFileException ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Config file '{Definitions.PipelineConfigFilename}' is invalid. Error parsing: {ex.Message}");
            }
            catch (PipelineNameRepeatedException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Pipeline name repeated");
            }
            catch (PipelineNotFoundException)
            {
                ExceptionManager.RaiseException(_loggerService, $"Can't find any pipeline with this name");
            }
            catch (Exception ex)
            {
                ExceptionManager.RaiseException(_loggerService, $"Throwed uncatched exception: {ex.ToString()}");
            }
            finally
            {
                _loggerService.Log("###### FINISHED! ######");
            }

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

        private static void CommandManager_OnLog(object sender, Events.LogEventArgs e)
        {
            _loggerService.Log(e.Log);
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
            ITemplateReplacementService templateReplacementService = new TemplateReplacementService();

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

            Register(new Commands.DD.AddPipelineCommand(storedDataService, fileService));
            Register(new Commands.DD.DeletePipelineCommand(storedDataService));
            Register(new Commands.DD.ShowPipelinesCommand(storedDataService));
            Register(new Commands.Windows.MovePathCommand(fileService));
            Register(new Commands.DD.ConfirmCommand());

            Register(new Commands.Dev.DotNet.AddWPFUserControlCommand(fileService, templateReplacementService));
            //Last commands for register
            Register(new Commands.DD.AddAliasCommand(storedDataService, commandManager.Commands));

            Register(new Commands.DD.PipelineCommand(commandManager.Commands, _loggerService, fileService, registryService, cryptoService, storedDataService));
            Register(new Commands.HelpCommand(commandManager.Commands));
        }

        private static void Register(CommandBase command)
        {
            commandManager.RegisterCommand(command);
        }
    }

}
