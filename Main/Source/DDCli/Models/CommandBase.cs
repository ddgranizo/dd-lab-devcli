using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Models
{
    public abstract class CommandBase
    {
        private const string BaseNamespace = "DDCli.Commands.";

        public event OnLogHnadler OnLog;
        public List<CommandParameterDefinition> CommandParametersDefinition { get; set; }
        public string CommandName { get; set; }
        public string CommandNameSpace { get; set; }
        public string Description { get; set; }

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
            CommandName = commandName;
            Description = description;
            CommandParametersDefinition = commandParameters ?? throw new ArgumentNullException(nameof(commandParameters));

            ImplementHelpCommand();
        }


        public CommandBase(string commandNameSpace, string commandName, string description)
            : this(commandNameSpace, commandName, description, new List<CommandParameterDefinition>())
        {

        }


        public void ImplementHelpCommand()
        {
            var helpCommand = new CommandParameterDefinition("help", CommandParameterDefinition.TypeValue.Boolean, "Details about the command and it's available parameters");
            CommandParametersDefinition.Add(helpCommand);
        }


        public bool CheckAndExecuteHelpCommand(List<CommandParameter> parameters)
        {
            if (parameters.Count == 1 && parameters[0].ParameterName == "help")
            {
                ExecuteHelp();
                return true;
            }
            return false;
        }

        public string GetInvocationCommandName()
        {
            return string.Format("{0}.{1}", CommandNameSpace, CommandName)
                    .Substring(0, CommandNameSpace.Length + CommandName.Length + 1 - "command".Length)
                    .Replace(".", "-").ToLowerInvariant();
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
            data.AppendLine("Parameters:");
            foreach (var item in CommandParametersDefinition)
            {
                data.AppendLine($"\t--{item.Name}, Type value: {item.Type.ToString()}, Description: {item.Description}");
            }
            Log(data.ToString());
        }

        public abstract void Execute(List<CommandParameter> parameters);

        public abstract bool CanExecute(List<CommandParameter> parameters);


        public bool IsParamOk(List<CommandParameter> parameters, string name)
        {
            var paremeterDefinition = CommandParametersDefinition.FirstOrDefault(k => k.Name == name);
            if (paremeterDefinition == null)
            {
                throw new KeyNotFoundException($"Parameter with name '{name}' doesnt exist in the definition of '{GetInvocationCommandName()}'");
            }
            var parameter = parameters.FirstOrDefault(k => k.ParameterName == name);
            if (parameter!= null)
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
    }
}
