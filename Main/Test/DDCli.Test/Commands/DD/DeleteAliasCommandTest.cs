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
    public class DeleteAliasCommandTest
    {
        public DeleteAliasCommandTest()
        {

        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeleteAliasCommandTest")]
        public void WhenExecuteCommand_CommandManager_ShouldAddAlias()
        {
            string aliasName = "myalias";
            string commandName = "mycommand";
            string commandNamespace = "name.space";
            string commandDescription = "description";

            var storedDataService = new StoredDataServiceMock(true);

            var instance = new CommandManager();
            instance.RegisterCommand(new CommandMock(commandNamespace, commandName, commandDescription));

            var commandDefinition = new DeleteAliasCommand(storedDataService);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            instance.ExecuteInputRequest(inputRequest);

            var storedAlias = storedDataService.DeletedAlias;

            var actual = storedAlias == aliasName;
            Assert.True(actual);
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeleteAliasCommandTest")]
        public void WhenExecuteCommandWithNonExistingAliasParameter_CommandManager_ShouldThrowException()
        {
            string aliasName = "myalias";
            var storedDataService = new StoredDataServiceMock(false);

            var instance = new CommandManager();

            var commandDefinition = new DeleteAliasCommand(storedDataService);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            Assert.Throws<AliasNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }
      

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeleteAliasCommandTest")]
        public void WhenExecuteCommandWithoutAliasParameter_CommandManager_ShouldThrowException()
        {
            var storedDataService = new StoredDataServiceMock(false);
            var commandDefinition = new DeleteAliasCommand(storedDataService);

            var instance = new CommandManager();
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }
    }
}
