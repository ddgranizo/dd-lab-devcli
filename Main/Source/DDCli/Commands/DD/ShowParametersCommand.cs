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
    public class ShowParametersCommand : CommandBase
    {
        public const string ParameterListHeaderDisplay = "Registered parameters:";
        public const string ParameterListFirstCharLine = "#";
        public const string ZeroRegisteredMessage = "Zero parameters registered";
        public static string HelpDefinition { get; private set; } = "Show all registered parameters";
        public IStoredDataService StoredDataService { get; }

        public ShowParametersCommand(
            IStoredDataService storedDataService)
            : base(typeof(ShowParametersCommand).Namespace, nameof(ShowParametersCommand), HelpDefinition)
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
            var parametersRegistered = StoredDataService.GetParametersWithValues().OrderBy(k => k).ToList();
            if (parametersRegistered.Count > 0)
            {
                Log(parametersRegistered.ToDisplayList(ParameterListHeaderDisplay, ParameterListFirstCharLine));
            }
            else
            {
                Log(ZeroRegisteredMessage);
            }
        }
    }
}
