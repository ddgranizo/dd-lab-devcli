using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Models
{
    public abstract class CommandBase
    {
        public event OnReplacedParameterHandler OnReplacedAutoIncrementInSubCommand;

        private const string BaseNamespace = "DDCli.Commands";

        public event OnLogHnadler OnLog;
        public List<CommandParameterDefinition> CommandParametersDefinition { get; set; }
        public string CommandName { get; set; }
        public string CommandNameSpace { get; set; }
        public string Description { get; set; }
        public IConsoleService ConsoleService { get; set; }
        public CommandBase(string commandNameSpace, string commandName, string description, List<CommandParameterDefinition> commandParameters)
        {

            if (string.IsNullOrEmpty(commandNameSpace))
            {
                throw new ArgumentException("commandNameSpace cannot be empty or null", nameof(commandNameSpace));
            }

            if (string.IsNullOrEmpty(commandName))
            {
                throw new ArgumentException("commandName cannot be empty or null", nameof(commandName));
            }

            if (string.IsNullOrEmpty(description))
            {
                throw new ArgumentException("help cannot be empty or null", nameof(description));
            }

            CommandNameSpace = commandNameSpace.Replace(BaseNamespace, string.Empty);
            if (CommandNameSpace.Length>0)
            {
                CommandNameSpace = CommandNameSpace.Substring(1); //remove the first dot
            }
            CommandName = commandName;
            Description = description;
            CommandParametersDefinition = commandParameters ?? throw new ArgumentNullException(nameof(commandParameters));

            ImplementHelpCommand();
        }


        public CommandBase(string commandNameSpace, string commandName, string description)
            : this(commandNameSpace, commandName, description, new List<CommandParameterDefinition>())
        {

        }


        public void RegisterCommandParameter(CommandParameterDefinition parameter)
        {
            this.CommandParametersDefinition.Add(parameter);
        }


        public void ImplementHelpCommand()
        {
            var helpCommand = new CommandParameterDefinition("help", CommandParameterDefinition.TypeValue.Boolean, "Details about the command and it's available parameters");
            CommandParametersDefinition.Add(helpCommand);
        }


        public bool IsHelpCommand(List<CommandParameter> parameters)
        {
            return parameters.Count == 1 && parameters[0].ParameterName == "help";
        }

        public void ExecuteHelpCommand(List<CommandParameter> parameters)
        {
            ExecuteHelp();
        }

        public string GetInvocationCommandName()
        {
            return !string.IsNullOrEmpty(CommandNameSpace)
                ? string.Format("{0}.{1}", CommandNameSpace, CommandName)
                    .Substring(0, CommandNameSpace.Length + CommandName.Length + 1 - "command".Length)
                    .Replace(".", "-").ToLowerInvariant()
                : CommandName
                    .Substring(0, CommandName.Length - "command".Length)
                    .Replace(".", "-").ToLowerInvariant();
        }

        public void ParameterAutoIncrementeReplaced(string parameterKey)
        {
            OnReplacedAutoIncrementInSubCommand?.Invoke(this, new Events.ReplacedParameterEventArgs(parameterKey));
        }


        public void Log(string log)
        {
            OnLog?.Invoke(this, new Events.LogEventArgs(log));
        }

        public void ExecuteHelp()
        {
            var data = new StringBuilder();
            data.AppendLine($"# Namespace: {CommandNameSpace}");
            data.AppendLine($"# Command Name: {CommandName}");
            data.AppendLine($"# Invocation Name: {GetInvocationCommandName()}");
            data.AppendLine($"# Description: {Description}");
            data.AppendLine(
                CommandParametersDefinition.ToDisplayList(
                    item => {
                        StringBuilder line = new StringBuilder();
                        line.Append($"--{item.Name}");
                        if (item.ShortCut != null)
                        {
                            line.Append($" [-{item.ShortCut}]");
                        }
                        line.Append($", Type value: {item.Type.ToString()}, Description: {item.Description}");
                        return line.ToString();
                    }, "Parameters:", ""));
            Log(data.ToString());
        }

        public abstract void Execute(List<CommandParameter> parameters);

        public abstract bool CanExecute(List<CommandParameter> parameters);


        public bool IsParamOk(List<CommandParameter> parameters, string name)
        {
            var paremeterDefinition = CommandParametersDefinition
                .FirstOrDefault(k => k.Name == name);
            if (paremeterDefinition == null)
            {
                throw new KeyNotFoundException($"Parameter with name '{name}' doesnt exist in the definition of '{GetInvocationCommandName()}'");
            }
            var parameter = parameters.FirstOrDefault(k => k.ParameterName == name);
            if (parameter != null)
            {
                if (paremeterDefinition.Type == CommandParameterDefinition.TypeValue.Boolean)
                {
                    return true;
                }
                else if (paremeterDefinition.Type == CommandParameterDefinition.TypeValue.Decimal)
                {
                    return true;
                }
                else if (paremeterDefinition.Type == CommandParameterDefinition.TypeValue.Integer)
                {
                    return true;
                }
                else if (paremeterDefinition.Type == CommandParameterDefinition.TypeValue.String)
                {
                    return !string.IsNullOrEmpty(parameter.ValueString);
                }
                else if (paremeterDefinition.Type == CommandParameterDefinition.TypeValue.Guid)
                {
                    return parameter.ValueGuid != Guid.Empty;
                }
            }
            return false;
        }


        public string GetStringParameterValue(List<CommandParameter> parameters, string name, string defaultValue = "")
        {
            return IsParamOk(parameters, name) ? parameters.FirstOrDefault(k => k.ParameterName == name).ValueString : defaultValue;
        }

        public bool GetBoolParameterValue(List<CommandParameter> parameters, string name, bool defaultValue = false)
        {
            return IsParamOk(parameters, name) ? parameters.FirstOrDefault(k => k.ParameterName == name).ValueBool : defaultValue;
        }

        public int GetIntParameterValue(List<CommandParameter> parameters, string name, int defaultValue = 0)
        {
            return IsParamOk(parameters, name) ? parameters.FirstOrDefault(k => k.ParameterName == name).ValueInt : defaultValue;
        }

        public decimal GetDecimalParameterValue(List<CommandParameter> parameters, string name, decimal defaultValue = 0)
        {
            return IsParamOk(parameters, name) ? parameters.FirstOrDefault(k => k.ParameterName == name).ValueDecimal : defaultValue;
        }

        public Guid GetGuidParameterValue(List<CommandParameter> parameters, string name)
        {
            return IsParamOk(parameters, name) ? parameters.FirstOrDefault(k => k.ParameterName == name).ValueGuid : Guid.Empty;
        }

    }
}
