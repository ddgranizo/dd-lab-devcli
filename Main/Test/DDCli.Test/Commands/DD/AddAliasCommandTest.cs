﻿using DDCli.Commands.DD;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Test.Mock;
using Moq;
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

            var storedDataService = new Mock<IStoredDataService>();

            var storedAlias = false;

            storedDataService.Setup(k => k.ExistsAlias(aliasName)).Returns(false);
            storedDataService.Setup(k => k.AddAlias(commandName, aliasName)).Callback(() => { storedAlias = true; });

            var instance = new CommandManager();
            instance.RegisterCommand(new MockCommand(commandNamespace, commandName, commandDescription));

            var commandDefinition = new AddAliasCommand(storedDataService.Object, instance.Commands);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                commandName,
                commandDefinition.CommandAliasParameter.GetInvokeName(),
                aliasName);

            instance.ExecuteInputRequest(inputRequest);

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
            var storedDataService = new Mock<IStoredDataService>();
            storedDataService.Setup(k => k.ExistsAlias(aliasName)).Returns(true);

            var instance = new CommandManager();

            var commandDefinition = new AddAliasCommand(storedDataService.Object, instance.Commands);
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
            var storedDataService = new Mock<IStoredDataService>();
            storedDataService.Setup(k => k.ExistsAlias(aliasName)).Returns(true);


            var instance = new CommandManager();
            instance.RegisterCommand(new MockCommand(commandNamespace, commandName, commandDescription));


            var commandDefinition = new AddAliasCommand(storedDataService.Object, instance.Commands);
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

            var storedDataService = new Mock<IStoredDataService>();
            var registeredCommands = new List<CommandBase>();
            var commandDefinition = new AddAliasCommand(storedDataService.Object, registeredCommands);

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

            var storedDataService = new Mock<IStoredDataService>();
            var registeredCommands = new List<CommandBase>();
            var commandDefinition = new AddAliasCommand(storedDataService.Object, registeredCommands);

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
