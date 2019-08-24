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
    public class ConfirmCommand : CommandBase
    {
        public static string HelpDefinition { get; private set; } = "Prompt a question so the user need to confirm";
        public CommandParameterDefinition CommandQuestionParameter { get; set; }

        public ConfirmCommand()
            : base(typeof(ConfirmCommand).Namespace, nameof(ConfirmCommand), HelpDefinition)
        {
            CommandQuestionParameter = new CommandParameterDefinition(
                "question",
                CommandParameterDefinition.TypeValue.String,
                "Question to prompt to the user",
                "q");

            RegisterCommandParameter(CommandQuestionParameter);

        }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return IsParamOk(parameters, CommandQuestionParameter.Name);
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var question = GetStringParameterValue(parameters, CommandQuestionParameter.Name);
            Log($"{question}\r\nPlease, confirm with 'yes'");
            var response = ConsoleService.ReadLine();
            if (new string[] {"yes","y","si","s" }.ToList().IndexOf(response.ToLowerInvariant())==-1)
            {
                throw new NotConfirmedException();
            }
        }
    }
}
