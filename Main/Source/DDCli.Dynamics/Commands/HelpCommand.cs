using DDCli.Extensions;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Commands
{
    public class HelpCommand: CommandBase
    {
        private const string HelpDefinition = "Shows all the available commands";
        public const string HeaderListMessage = "Available commands:";
        public const string FirstCharacterLine = "#";
        public List<CommandBase> Commands { get; }

        public HelpCommand(List<CommandBase> commands)
            : base(typeof(HelpCommand).Namespace, nameof(HelpCommand), HelpDefinition)
        {
            Commands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            Log(GetMessage());
        }

        private string GetMessage()
        {
            var data = new StringBuilder();
            Version assemblyVersion = Assembly.GetEntryAssembly().GetName().Version;
            data.AppendLine($"DDCli version {assemblyVersion.ToString()}");
            data.AppendLine(
                Commands
                    .OrderBy(k => k.GetInvocationCommandName())
                    .ToDisplayList((item) => {
                        return item.GetInvocationCommandName();
                    },
                        HeaderListMessage,
                        FirstCharacterLine));
            return data.ToString();
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }
    }
}
