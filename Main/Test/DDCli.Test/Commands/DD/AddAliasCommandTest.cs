using DDCli.Commands.DD;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Test.Mock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace DDCli.Test.Commands.DD
{
    public class AddAliasCommandTest
    {
        public AddAliasCommandTest()
        {

        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddAliasCommandTest")]
        public void WhenExecuteCommand_CommandManager_ShouldAddAlias()
        {
            string aliasName = "myalias";
            string commandName = "mycommand";
            string commandNamespace = "name.space";
            string commandDescription = "description";

            var storedDataService = new StoredDataServiceMock(false);


            var instance = new CommandManager();
            instance.RegisterCommand(new CommandMock(commandNamespace, commandName, commandDescription));

            var commandDefinition = new AddAliasCommand(storedDataService, instance.Commands);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                commandName,
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            instance.ExecuteInputRequest(inputRequest);

            var storedAlias = storedDataService.AddedCommand == commandName;

            var actual = storedAlias;
            Assert.True(actual);
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddAliasCommandTest")]
        public void WhenExecuteCommandWhichNotExtis_CommandManager_ShouldThrowException()
        {
            string aliasName = "myalias";
            string commandName = "mycommand";
            var storedDataService = new StoredDataServiceMock(true);

            var instance = new CommandManager();

            var commandDefinition = new AddAliasCommand(storedDataService, instance.Commands);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                commandName,
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            Assert.Throws<CommandNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddAliasCommandTest")]
        public void WhenExecuteCommandWithRepeatedAliasParameter_CommandManager_ShouldThrowException()
        {
            string aliasName = "myalias";
            string commandName = "mycommand";
            string commandNamespace = "name.space";
            string commandDescription = "description";
            var storedDataService = new StoredDataServiceMock(true);


            var instance = new CommandManager();
            instance.RegisterCommand(new CommandMock(commandNamespace, commandName, commandDescription));


            var commandDefinition = new AddAliasCommand(storedDataService, instance.Commands);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                commandName,
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            Assert.Throws<AliasRepeatedException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddAliasCommandTest")]
        public void WhenExecuteCommandWithoutAliasParameter_CommandManager_ShouldThrowException()
        {

            string commandName = "mycommand";

            var storedDataService = new  StoredDataServiceMock(false);
            var registeredCommands = new List<CommandBase>();
            var commandDefinition = new AddAliasCommand(storedDataService, registeredCommands);

            var instance = new CommandManager();
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                commandName);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddAliasCommandTest")]
        public void WhenExecuteCommandWithoutCommandParameter_CommandManager_ShouldThrowException()
        {

            string aliasName = "myalias";

            var storedDataService = new StoredDataServiceMock(false);
            var registeredCommands = new List<CommandBase>();
            var commandDefinition = new AddAliasCommand(storedDataService, registeredCommands);

            var instance = new CommandManager();
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }
    }
}
