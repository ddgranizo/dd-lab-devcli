using DDCli.Commands.DD;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DDCli.Test.Commands.DD
{
    public class EchoCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        readonly LoggerServiceMock _loggerServiceMock;

        public string LastLog { get; set; }
        public EchoCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "EchoCommandTest")]
        public void WhenExecuteCommandText_CommandManager_ShouldLogText()
        {
            var cmd = "hellomoto";
            var storedDataService = new StoredDataServiceMock();
            var commandDefinition = new EchoCommand();

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandTextParameter.GetInvokeName(),
                cmd);
            instance.ExecuteInputRequest(inputRequest);

            var expected = cmd;
            var actual = _loggerServiceMock.Logs.First();

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "EchoCommandTest")]
        public void WhenExecuteCommandWithoutText_CommandManager_ShouldThrowException()
        {
            var storedDataService = new StoredDataServiceMock();
            var commandDefinition = new EchoCommand();

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
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
            _loggerServiceMock.Log(e.Log);
        }
    }
}
