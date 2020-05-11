using DDCli.Exceptions;
using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.DD
{
    public class ShowComandsHistoryCommand : CommandBase
    {
        public CommandParameterDefinition CountParameter { get; set; }


        public const string ListHeaderDisplay = "History commands:";
        public const string ListFirstCharLine = "#";
        public const string ZeroRegisteredMessage = "Zero commands found";
        public static string HelpDefinition { get; private set; } = "Show all history commands";
        public IStoredDataService StoredDataService { get; }

        public ShowComandsHistoryCommand(
            IStoredDataService storedDataService)
            : base(typeof(ShowComandsHistoryCommand).Namespace, nameof(ShowComandsHistoryCommand), HelpDefinition)
        {
            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));

            CountParameter = new CommandParameterDefinition("count",
                CommandParameterDefinition.TypeValue.Integer,
                "Count of records. Default value Count=10", "c");
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var count = GetIntParameterValue(parameters, CountParameter.Name, 10);
            var commands = StoredDataService.GetCommandsFromHistorical(count);
            if (commands.Count > 0)
            {
                Log(commands.ToDisplayList(k => $"{k.Command}", ListHeaderDisplay, ListFirstCharLine));
            }
            else
            {
                Log(ZeroRegisteredMessage);
            }
        }
    }
}
