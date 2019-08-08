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
    public class DeleteParameterCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;

        public DeleteParameterCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeleteParameterCommandTest")]
        public void WhenExecuteCommandWithParameter_CommandManager_ShouldDeleteParameter()
        {
            var key = "my.parameter";

            var storedDataService = new StoredDataServiceMock() { ReturnBoolExistsParameter = true };
            var commandDefinition = new DeleteParameterCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandKeyParameter.GetInvokeName(),
                key);

            instance.ExecuteInputRequest(inputRequest);

            var expected = key;
            var actual = storedDataService.DeletedParameter;

            Assert.Equal(expected, actual);
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeleteParameterCommandTest")]
        public void WhenExecuteCommandWithNotRegisteredParameter_CommandManager_ShouldThrowException()
        {
            var key = "my.parameter";

            var storedDataService = new StoredDataServiceMock() { ReturnBoolExistsParameter = false };
            var commandDefinition = new DeleteParameterCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandKeyParameter.GetInvokeName(),
                key);

            Assert.Throws<ParameterNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeleteParameterCommandTest")]
        public void WhenExecuteCommandWithoutCommandKey_CommandManager_ShouldThrowException()
        {


            var storedDataService = new StoredDataServiceMock();
            var commandDefinition = new DeleteParameterCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
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
