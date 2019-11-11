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
    public class AddPipelineCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        readonly ILoggerService _loggerServiceMock;

        public AddPipelineCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddPipelineCommandTest")]
        public void WhenExecuteCommandWithValidParameters_CommandManager_ShouldAddPipeline()
        {
            string pipelinePath = "myPath";
            string pipelineName = "myPipeline";
            var storedDataService = new StoredDataServiceMock() { ExistsPipelineReturn = false };
            var fileService = new FileServiceMock() { ExistsFileReturn = true};
            var commandDefinition = new AddPipelineCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                pipelinePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                pipelineName);
            
            instance.ExecuteInputRequest(inputRequest);

            var expectedPath = pipelinePath;
            var actualPath = storedDataService.AddedPipelinePath;
            Assert.Equal(expectedPath, actualPath);

            var expectedName = pipelineName;
            var actualName = storedDataService.AddedPipelineName;
            Assert.Equal(expectedName, actualName);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddPipelineCommandTest")]
        public void WhenExecuteCommandWithPipelineNonValidPipelineName_CommandManager_ShouldThrowException()
        {
            string pipelinePath = "myPath";
            string pipelineName = "myPipe NONVALID line";
            var storedDataService = new StoredDataServiceMock() { ExistsPipelineReturn = false };
            var fileService = new FileServiceMock() { ExistsFileReturn = true };
            var commandDefinition = new AddPipelineCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                pipelinePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                pipelineName);

            Assert.Throws<InvalidStringFormatException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


      


        [Fact]
        [Trait("TestCategory", "UnitTest"),
         Trait("TestCategory", "CommandTest"),
         Trait("TestCategory", "DDCommandTest"),
         Trait("TestCategory", "AddPipelineCommandTest")]
        public void WhenExecuteCommandWithPipelineNonExistingPath_CommandManager_ShouldThrowException()
        {
            string pipelinePath = "myPath";
            string pipelineName = "myPipeline";
            var storedDataService = new StoredDataServiceMock() { ExistsPipelineReturn = false };
            var fileService = new FileServiceMock() { ExistsFileReturn = false };
            var commandDefinition = new AddPipelineCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                pipelinePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                pipelineName);

            Assert.Throws<PathNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddPipelineCommandTest")]
        public void WhenExecuteCommandWithPipelineNameRepeated_CommandManager_ShouldThrowException()
        {
            string pipelinePath = "myPath";
            string pipelineName = "myPipeline";
            var storedDataService = new StoredDataServiceMock() { ExistsPipelineReturn = true};
            var fileService = new FileServiceMock() {ExistsFileReturn = true };
            var commandDefinition = new AddPipelineCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                pipelinePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                pipelineName);

            Assert.Throws<PipelineNameRepeatedException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddPipelineCommandTest")]
        public void WhenExecuteCommandWithoutNameParameter_CommandManager_ShouldThrowException()
        {
            string pipelinePath= "myPath";

            var storedDataService = new StoredDataServiceMock();
            var fileService = new FileServiceMock();
            var commandDefinition = new AddPipelineCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                pipelinePath);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddPipelineCommandTest")]
        public void WhenExecuteCommandWithoutPathParameter_CommandManager_ShouldThrowException()
        {
            string pipelineName = "myPipeline";

            var storedDataService = new StoredDataServiceMock();
            var fileService = new FileServiceMock();
            var commandDefinition = new AddPipelineCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                pipelineName);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }
    }
}
