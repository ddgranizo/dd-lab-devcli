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
    public class DeletePipelineCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        readonly ILoggerService _loggerServiceMock;

        public DeletePipelineCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
                 Trait("TestCategory", "CommandTest"),
                 Trait("TestCategory", "DDCommandTest"),
                 Trait("TestCategory", "DeletePipelineCommandTest")]
        public void WhenExecuteCommandWithValidPipeline_CommandManager_ShouldThrowException()
        {
            string pipelineName = "myPipeline";
            var storedDataService = new StoredDataServiceMock() { ExistsPipelineReturn = true };
            var commandDefinition = new DeletePipelineCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                pipelineName);
            
            instance.ExecuteInputRequest(inputRequest);

            var expected = pipelineName;
            var actual = storedDataService.DeletedPipeline;

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
         Trait("TestCategory", "CommandTest"),
         Trait("TestCategory", "DDCommandTest"),
         Trait("TestCategory", "DeletePipelineCommandTest")]
        public void WhenExecuteCommandWithNonExistingPipeline_CommandManager_ShouldThrowException()
        {
            string pipelineName = "myPipeline";
            var storedDataService = new StoredDataServiceMock() { ExistsPipelineReturn = false };
            var commandDefinition = new DeletePipelineCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                pipelineName);

            Assert.Throws<PipelineNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeletePipelineCommandTest")]
        public void WhenExecuteCommandWithoutNameParameter_CommandManager_ShouldThrowException()
        {

            var storedDataService = new StoredDataServiceMock();
            var commandDefinition = new DeletePipelineCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
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
