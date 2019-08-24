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
    public class ConfirmCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        public string LastLog { get; set; }
        public ConfirmCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "EchoCommandTest")]
        public void WhenExecuteCommandText_CommandManager_ShouldLogText()
        {
            var question = "myQuestion";
            var storedDataService = new StoredDataServiceMock();
            var consoleServiceMock = new ConsoleServiceMock();

            consoleServiceMock.ReadLineReturn = "invalidYes";
            var commandDefinition = new ConfirmCommand(consoleServiceMock);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandQuestionParameter.GetInvokeName(),
                question);

            Assert.Throws<NotConfirmedException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "EchoCommandTest")]
        public void WhenExecuteCommandWithoutQuestion_CommandManager_ShouldThrowException()
        {
            var storedDataService = new StoredDataServiceMock();

            var consoleServiceMock = new ConsoleServiceMock();

            var commandDefinition = new ConfirmCommand(consoleServiceMock);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        private void Instance_OnLog(object sender, Events.LogEventArgs e)
        {
            LastLog = e.Log;
        }
    }
}
