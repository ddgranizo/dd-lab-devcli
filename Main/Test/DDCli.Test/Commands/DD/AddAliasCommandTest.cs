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

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;


        public AddAliasCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
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

            var mockCommand = new CommandMock(commandNamespace, commandName, commandDescription);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(mockCommand);

            var commandDefinition = new AddAliasCommand(storedDataService, instance.Commands);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                mockCommand.GetInvocationCommandName(),
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            instance.ExecuteInputRequest(inputRequest);

            var storedAlias = storedDataService.AddedCommand == mockCommand.GetInvocationCommandName();

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

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);

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

            var mockCommand = new CommandMock(commandNamespace, commandName, commandDescription);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(mockCommand);


            var commandDefinition = new AddAliasCommand(storedDataService, instance.Commands);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                mockCommand.GetInvocationCommandName(),
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

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
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

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
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
