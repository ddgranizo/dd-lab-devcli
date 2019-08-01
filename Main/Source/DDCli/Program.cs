using DDCli.Exceptions;
using DDCli.Models;
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
            RegisterCommands();
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
            catch (Exception ex)
            {
                Console.WriteLine($"Throwed uncatched exception: {ex.ToString()}");
            }
        }

        private static void CommandManager_OnLog(object sender, Events.LogEventArgs e)
        {
            Console.WriteLine(e.Log);
        }

        private static void RegisterCommands()
        {
            Register(new Commands.Dev.Git.CSharpGitIgnoreCommand());
            Register(new Commands.Dev.DotNet.PublishDebugWinCommand());
            Register(new Commands.Dev.DotNet.PublishReleaseWinCommand());
            Register(new Commands.Dev.DotNet.OpenVisualStudioCommand());
            Register(new Commands.Dev.Windows.OpenRepoCommand());
            Register(new Commands.Dev.Utils.SetAlias());
        }

        private static void Register(CommandBase command)
        {
            commandManager.RegisterCommand(command);
        }
    }

}
