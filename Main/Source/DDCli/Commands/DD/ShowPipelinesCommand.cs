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
    public class ShowPipelinesCommand : CommandBase
    {
        public const string ListHeaderDisplay = "Registered pipelines:";
        public const string ListFirstCharLine = "#";
        public const string ZeroRegisteredMessage = "Zero pipelines registered";
        public static string HelpDefinition { get; private set; } = "Show all registered pipelines";
        public IStoredDataService StoredDataService { get; }

        public ShowPipelinesCommand(
            IStoredDataService storedDataService)
            : base(typeof(ShowPipelinesCommand).Namespace, nameof(ShowPipelinesCommand), HelpDefinition)
        {
            StoredDataService = storedDataService
                ?? throw new ArgumentNullException(nameof(storedDataService));
        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var pipelines = StoredDataService.GetPipelines().OrderBy(k => k.PipelineName).ToList();
            if (pipelines.Count > 0)
            {
                Log(pipelines.ToDisplayList(k => $"{k.PipelineName} => {k.Description} located at {k.Path}", ListHeaderDisplay, ListFirstCharLine));
            }
            else
            {
                Log(ZeroRegisteredMessage);
            }
        }
    }
}
