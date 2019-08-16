using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Commands.Crm
{
    public class ImportSolutionCommand : CommandBase
    {
        private const string HelpDefinition = @"Import solution to environment";

        public CommandParameterDefinition CommandStringConnectionParameter { get; set; }
        public CommandParameterDefinition CommandPathParameter { get; set; }

        public ImportSolutionCommand()
            : base(typeof(ImportSolutionCommand).Namespace, nameof(ImportSolutionCommand), HelpDefinition)
        {
            CommandPathParameter = new CommandParameterDefinition(
                "path",
                CommandParameterDefinition.TypeValue.String,
                "Indicates the path (file or directory) for zip",
                "p");

            CommandParametersDefinition.Add(CommandPathParameter);

        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            throw new NotImplementedException();
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
