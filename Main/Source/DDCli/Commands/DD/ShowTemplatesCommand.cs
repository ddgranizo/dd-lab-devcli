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
    public class ShowTemplatesCommand : CommandBase
    {
        public const string ListHeaderDisplay = "Registered templates:";
        public const string ListFirstCharLine = "#";
        public const string ZeroRegisteredMessage = "Zero templates registered";
        public static string HelpDefinition { get; private set; } = "Show all registered templates";
        public IStoredDataService StoredDataService { get; }

        public ShowTemplatesCommand(
            IStoredDataService storedDataService)
            : base(typeof(ShowTemplatesCommand).Namespace, nameof(ShowTemplatesCommand), HelpDefinition)
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
            var templates = StoredDataService.GetTemplates().OrderBy(k => k.TemplateName).ToList();
            if (templates.Count > 0)
            {
                Log(templates.ToDisplayList(k => $"{k.TemplateName} => {k.Description} located at {k.Path}", ListHeaderDisplay, ListFirstCharLine));
            }
            else
            {
                Log(ZeroRegisteredMessage);
            }
        }
    }
}
