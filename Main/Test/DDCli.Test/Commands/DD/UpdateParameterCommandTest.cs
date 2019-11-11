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
    public class UpdateParameterCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        readonly ILoggerService _loggerServiceMock;

        public UpdateParameterCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "UpdateParameterCommandTest")]
        public void WhenExecuteCommandWithoutRegisteredKeyAndValue_CommandManager_ShouldUpdate()
        {

            string key = "my.param";
            string value = "my new value";
            var storedDataService = new StoredDataServiceMock() {
                ReturnBoolExistsParameter = true
            };
            var commandDefinition = new UpdateParameterCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandKeyParameter.GetInvokeName(),
                key,
                commandDefinition.CommandValueParameter.GetInvokeName(),
                value);
            instance.ExecuteInputRequest(inputRequest);

            var expectedKey = key;
            var actualKey = storedDataService.UpdatedParameterKey;

            Assert.Equal(expectedKey, actualKey);

            var expectedValue = value;
            var actualValue = storedDataService.UpdatedParameterValue;

            Assert.Equal(expectedValue, actualValue);
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "UpdateParameterCommandTest")]
        public void WhenExecuteCommandWithoutCommandValue_CommandManager_ShouldThrowException()
        {

            string key = "my.param";

            var storedDataService = new StoredDataServiceMock();
            var commandDefinition = new UpdateParameterCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandKeyParameter.GetInvokeName(),
                key);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "UpdateParameterCommandTest")]
        public void WhenExecuteCommandWithoutCommandKey_CommandManager_ShouldThrowException()
        {

            string value = "my value";

            var storedDataService = new StoredDataServiceMock();
            var commandDefinition = new UpdateParameterCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandValueParameter.GetInvokeName(),
                value);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }
    }
}
