using DDCli.Events;
using DDCli.Exceptions;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DDCli
{
    public delegate void OnLogHnadler(object sender, LogEventArgs e);
    public class CommandManager
    {
        public event OnLogHnadler OnLog;
        public List<CommandBase> Commands { get; set; }

        public CommandManager()
        {
            Commands = new List<CommandBase>();
        }


        public void RegisterCommand(CommandBase command)
        {
            if (command.CommandName.Substring(command.CommandName.Length - "command".Length).ToLowerInvariant() != "command")
            {
                throw new NotImplementedException("Invalid command name");
            }
            Commands.Add(command);
        }


        public void ExecuteInputRequest(InputRequest inputRequest)
        {

            if (inputRequest.CommandNamespace == string.Empty && inputRequest.CommandName == "helpcommand")
            {
                Command_OnLog(this, new LogEventArgs(GetHelpMessage()));
                return;
            }

            var commands = Commands
                .Where(k =>
                    k.CommandName.ToLowerInvariant() == inputRequest.CommandName)
                    .ToList();
            if (commands.Count>1)
            {
                throw new DuplicateCommandException(commands.Select(k => k.GetInvocationCommandName()).ToList());
            }

            if (commands.Count == 0)
            {
                throw new CommandNotFoundException($"{inputRequest.CommandNamespace}.{inputRequest.CommandName}");
            }

            var command = commands[0];

            var commandsParameters = new List<CommandParameter>();
            foreach (var parameterDefinition in command.CommandParametersDefinition)
            {
                var itemInput = GetImputParameterFromRequest(inputRequest, parameterDefinition);
                if (itemInput != null)
                {
                    var parameter = GetParsedCommandParameter(command, parameterDefinition, itemInput);
                    commandsParameters.Add(parameter);
                }
            }

            if (command.CanExecute(commandsParameters))
            {
                try
                {
                    var timer = new Stopwatch(); timer.Start();
                    command.OnLog += Command_OnLog;
                    if (!command.CheckAndExecuteHelpCommand(commandsParameters))
                    {
                        command.Execute(commandsParameters);
                    }
                    Log($"Executed command in {timer.ElapsedMilliseconds}ms");
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
            else
            {
                throw new InvalidParamsException();
            }
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

        public void Log(string log)
        {
            OnLog?.Invoke(this, new LogEventArgs(log));
        }

        private void Command_OnLog(object sender, LogEventArgs e)
        {
            OnLog?.Invoke(sender, e);
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
            return new string[] {
                "true",
                "yes",
                "1",
                "si"
            }.ToList().IndexOf(value.ToLowerInvariant()) > -1;
        }

        private string GetHelpMessage()
        {
            var data = new StringBuilder();
            data.AppendLine("Available commands:");
            foreach (var item in Commands.OrderBy(k => k.GetInvocationCommandName()))
            {
                data.AppendLine($"\t#{item.GetInvocationCommandName()}");
            }
            return data.ToString();
        }

    }
}
