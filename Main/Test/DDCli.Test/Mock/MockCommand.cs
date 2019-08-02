using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class MockCommand : CommandBase
    {

        public Func<List<CommandParameter>, bool> CanExecuteFunction { get; set; }
        public bool CanExecuteReturn { get; set; }

        public Action<List<CommandParameter>> ExecuteAction { get; set; }


        public MockCommand(string nameSpace, string commandName, string decription) : base(nameSpace, commandName, decription) { }


        public MockCommand(string nameSpace, string commandName, string decription, bool canExecuteReturn) 
            : base(nameSpace, commandName, decription)
        { CanExecuteReturn = canExecuteReturn; }

        public MockCommand(string nameSpace, string commandName, string decription, Func<List<CommandParameter>, bool> canExecuteFunction) 
            : base(nameSpace, commandName, decription)
        { CanExecuteFunction = canExecuteFunction; }

        public MockCommand(string nameSpace, string commandName, string decription, Action<List<CommandParameter>> executeAction) 
            : base(nameSpace, commandName, decription)
        { ExecuteAction = executeAction; }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return CanExecuteFunction != null
                ? CanExecuteFunction.Invoke(parameters)
                : CanExecuteReturn;
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            if (ExecuteAction != null)
            {
                ExecuteAction.Invoke(parameters);
            }
        }
    }
}
