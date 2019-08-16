using DDCli.Commands;
using DDCli.Events;
using DDCli.Exceptions;
using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Services;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DDCli
{
    public delegate void OnLogHnadler(object sender, LogEventArgs e);
    public class CommandManager
    {

        public event OnReplacedParameterHandler OnReplacedAutoIncrementInCommand;
        public enum ExecutionModeTypes
        {
            Single = 1,
            Multiple = 2,
        }

        public event OnLogHnadler OnLog;
        public List<CommandBase> Commands { get; set; }
        public IStoredDataService StoredDataService { get; }
        public ICryptoService CryptoService { get; }
        public ExecutionModeTypes ExecutionMode { get; }

        private const string MandatoryCommandSufix = "command";
        private readonly ParameterManager _parameterManager;

        private readonly List<string> _autoincrementParametersReplaced = new List<string>();
        public List<string> EncryptedResolved { get; set; }
        public CommandManager(
            IStoredDataService storedDataService,
            ICryptoService cryptoService,
            ExecutionModeTypes executionMode = ExecutionModeTypes.Single)
        {
            Commands = new List<CommandBase>();
            StoredDataService = storedDataService ?? throw new ArgumentNullException(nameof(storedDataService));
            CryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
            ExecutionMode = executionMode;
            _parameterManager = new ParameterManager(CryptoService);
            EncryptedResolved = new List<string>();
        }


        public void RegisterCommand(CommandBase command)
        {
            if (command.CommandName.Substring(command.CommandName.Length - MandatoryCommandSufix.Length).ToLowerInvariant() != MandatoryCommandSufix)
            {
                throw new NotImplementedException("Invalid command name");
            }
            Commands.Add(command);
        }

        public void RegisterCommands(List<CommandBase> commands)
        {
            foreach (var command in commands)
            {
                RegisterCommand(command);
            }
        }


        public void ExecuteInputRequest(InputRequest inputRequest, List<string> consoleInputs = null)
        {
            List<CommandBase> commands = SearchCommandAndAlias(inputRequest);

            if (commands.Count > 1)
            {
                throw new DuplicateCommandException(commands.Select(k => k.GetInvocationCommandName()).ToList());
            }

            if (commands.Count == 0)
            {
                throw new CommandNotFoundException($"{inputRequest.CommandNamespace}.{inputRequest.CommandName}");
            }

            var command = commands[0];

            var commandsParameters = new List<CommandParameter>();
            if (command.CommandParametersDefinition.Where(k => k.Name != "help").Count() == 1
                && inputRequest.InputParameters.Count == 1
                && inputRequest.InputParameters[0].IsOnlyOne)
            {
                var targetCommandParameter = command.CommandParametersDefinition.FirstOrDefault(k => k.Name != "help");
                var parameter = GetParsedCommandParameter(
                    command, targetCommandParameter, inputRequest.InputParameters[0]);
                commandsParameters.Add(parameter);
            }
            else
            {
                foreach (var parameterDefinition in command.CommandParametersDefinition)
                {
                    var itemInput = GetImputParameterFromRequest(inputRequest, parameterDefinition);
                    if (itemInput != null)
                    {
                        var parameter = GetParsedCommandParameter(command, parameterDefinition, itemInput);
                        commandsParameters.Add(parameter);
                    }
                }

            }

            if (command.CanExecute(commandsParameters) || command.IsHelpCommand(commandsParameters))
            {
                _parameterManager.OnReplacedEncrypted += _parameterManager_OnReplacedEncrypted;
                _parameterManager.OnReplacedAutoIncrement += _parameterManager_OnReplacedAutoIncrement;

                try
                {
                    ExecuteCommand(command, commandsParameters, consoleInputs);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    _parameterManager.OnReplacedEncrypted -= _parameterManager_OnReplacedEncrypted;
                    _parameterManager.OnReplacedAutoIncrement -= _parameterManager_OnReplacedAutoIncrement;
                    if (ExecutionMode == ExecutionModeTypes.Single)
                    {
                        StoredDataService.UpdateAutoIncrements(this._autoincrementParametersReplaced);
                    }
                }
            }
            else
            {
                throw new InvalidParamsException();
            }
        }

        private void ExecuteCommand(CommandBase command, List<CommandParameter> commandsParameters, List<string> consoleInputs = null)
        {
            try
            {
                command.OnLog += Command_OnLog;
                command.OnReplacedAutoIncrementInSubCommand += _parameterManager_OnReplacedAutoIncrement;
                if (command.CanExecute(commandsParameters))
                {

                    var processedParameters = _parameterManager.ResolveParameters(StoredDataService, commandsParameters);
                    var timer = new Stopwatch(); timer.Start();
                    command.ConsoleService = new ConsoleService(consoleInputs);
                    command.Execute(commandsParameters);
                    Console.WriteLine($"Executed command in {timer.ElapsedMilliseconds}ms");
                }
                else
                {
                    command.ExecuteHelp();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                command.OnLog -= Command_OnLog;
            }
        }

        private void _parameterManager_OnReplacedAutoIncrement(object sender, ReplacedParameterEventArgs args)
        {

            if (_autoincrementParametersReplaced.FirstOrDefault(k => args.Parameter == k) == null)
            {
                _autoincrementParametersReplaced.Add(args.Parameter);
            }
            OnReplacedAutoIncrementInCommand?.Invoke(sender, args);
        }

        private void _parameterManager_OnReplacedEncrypted(object sender, ReplacedParameterValueEventArgs args)
        {
            EncryptedResolved.Add(args.Value);
        }

        private List<CommandBase> SearchCommandAndAlias(InputRequest inputRequest)
        {
            var commands = Commands.Where(k => k.CommandName.ToLowerInvariant() == inputRequest.CommandName)
                                    .ToList();

            if (commands.Count == 0 && inputRequest.CommandName.Length > MandatoryCommandSufix.Length)
            {
                var aliasName = inputRequest.CommandName.Substring(0, inputRequest.CommandName.Length - MandatoryCommandSufix.Length);
                var isAlias = StoredDataService.ExistsAlias(aliasName);
                if (isAlias)
                {
                    var commandName = StoredDataService.GetAliasedCommand(aliasName);
                    commands = Commands.Where(k => k.GetInvocationCommandName() == commandName)
                                        .ToList();
                }
            }

            return commands;
        }

        private static InputParameter GetImputParameterFromRequest(InputRequest inputRequest, CommandParameterDefinition parameter)
        {
            var inputByName = inputRequest
                    .InputParameters
                    .FirstOrDefault(k => k.ParameterName == parameter.Name && !k.IsShortCut);
            if (inputByName != null)
            {
                return inputByName;
            }
            var inputByShortCut = inputRequest
                    .InputParameters
                    .FirstOrDefault(k => k.ParameterName == parameter.ShortCut && k.IsShortCut);
            return inputByShortCut;
        }


        private void Command_OnLog(object sender, LogEventArgs e)
        {
            var ofuscated = ObfuscateLogWithEncrypted(e.Log);
            OnLog?.Invoke(sender, new LogEventArgs(ofuscated));
        }


        private string ObfuscateLogWithEncrypted(string log)
        {
            var replacedLog = log;
            foreach (var item in EncryptedResolved)
            {
                replacedLog = replacedLog.Replace(item, Definitions.PasswordOfuscator);
            }
            return replacedLog;
        }

        private static CommandParameter GetParsedCommandParameter(CommandBase command, CommandParameterDefinition item, InputParameter itemInput)
        {
            var parameterName = item.Name;
            CommandParameter commandParameter = null;
            var rawString = itemInput.RawStringValue;
            if (item.Type == CommandParameterDefinition.TypeValue.String)
            {
                commandParameter = new CommandParameter(parameterName, rawString);
            }
            else if (item.Type == CommandParameterDefinition.TypeValue.Boolean)
            {
                if (!itemInput.HasValue)
                {
                    commandParameter = new CommandParameter(parameterName, true);
                }
                else
                {
                    bool boolValue = StrToBool(rawString);
                    commandParameter = new CommandParameter(parameterName, boolValue);
                }
            }
            else if (item.Type == CommandParameterDefinition.TypeValue.Decimal)
            {
                if (!decimal.TryParse(rawString, out decimal d))
                {
                    throw new InvalidCastException(GetInvalidCastExceptionMessage(command, item, rawString, "decimal"));
                }
                commandParameter = new CommandParameter(parameterName, d);
            }
            else if (item.Type == CommandParameterDefinition.TypeValue.Integer)
            {
                if (!int.TryParse(rawString, out int d))
                {
                    throw new InvalidCastException(GetInvalidCastExceptionMessage(command, item, rawString, "integer"));
                }
                commandParameter = new CommandParameter(parameterName, d);
            }
            else if (item.Type == CommandParameterDefinition.TypeValue.Guid)
            {
                if (!Guid.TryParse(rawString, out Guid d))
                {
                    throw new InvalidCastException(GetInvalidCastExceptionMessage(command, item, rawString, "guid"));
                }
                commandParameter = new CommandParameter(parameterName, d);
            }
            return commandParameter;

        }

        private static string GetInvalidCastExceptionMessage(CommandBase command, CommandParameterDefinition item, string rawString, string typeString)
        {
            return $"'{rawString}' cannot be converted to {typeString} for command {command.CommandName} and parameter ${item.Name}";
        }

        private static bool StrToBool(string value)
        {
            return Definitions
                    .AvailableTrueStrings
                    .ToList().IndexOf(value.ToLowerInvariant()) > -1;
        }


    }
}
