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
    public class ShowAliasCommand : CommandBase
    {
        public const string ZeroRegisteredMessage = "Zero registered alias";
        public const string HeaderForListingAlias = "Stored alias:";
        public const string FirstCharacterDefaultForListingAlias = "#";

        public static string HelpDefinition { get; private set; } = "Show all registered alias";
        public IStoredDataService StoredDataService { get; }

        public ShowAliasCommand(
            IStoredDataService storedDataService)
            : base(typeof(ShowAliasCommand).Namespace, nameof(ShowAliasCommand), HelpDefinition)
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
            var alias = StoredDataService.GetAliasWithCommand();

            if (alias.Count>0)
            {
                Log(alias.ToDisplayList(HeaderForListingAlias, FirstCharacterDefaultForListingAlias));
            }
            else
            {
                Log(ZeroRegisteredMessage);
            }
        }
    }
}
