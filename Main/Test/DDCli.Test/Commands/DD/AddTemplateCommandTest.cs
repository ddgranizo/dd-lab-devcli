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
    public class AddTemplateCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        readonly ILoggerService _loggerServiceMock;

        public AddTemplateCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddTemplateCommandTest")]
        public void WhenExecuteCommandWithValidParameters_CommandManager_ShouldAddTemplate()
        {
            string templatePath = "myPath";
            string templateName = "myTemplate";
            var storedDataService = new StoredDataServiceMock() { ExistsTemplateReturn = false };
            var fileService = new FileServiceMock() { ExistsDirectoryReturn = true, ExistsTemplateConfigFileReturn = true };
            var commandDefinition = new AddTemplateCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                templatePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);
            
            instance.ExecuteInputRequest(inputRequest);

            var expectedPath = templatePath;
            var actualPath = storedDataService.AddedTemplatePath;
            Assert.Equal(expectedPath, actualPath);

            var expectedName = templateName;
            var actualName = storedDataService.AddedTemplateName;
            Assert.Equal(expectedName, actualName);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddTemplateCommandTest")]
        public void WhenExecuteCommandWithTemplateNonValidTemplateName_CommandManager_ShouldThrowException()
        {
            string templatePath = "myPath";
            string templateName = "myTem NONVALID plate";
            var storedDataService = new StoredDataServiceMock() { ExistsTemplateReturn = false };
            var fileService = new FileServiceMock() { ExistsDirectoryReturn = true, ExistsTemplateConfigFileReturn = true };
            var commandDefinition = new AddTemplateCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                templatePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);

            Assert.Throws<InvalidStringFormatException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
                 Trait("TestCategory", "CommandTest"),
                 Trait("TestCategory", "DDCommandTest"),
                 Trait("TestCategory", "AddTemplateCommandTest")]
        public void WhenExecuteCommandWithTemplateNonExistingTemplateFile_CommandManager_ShouldThrowException()
        {
            string templatePath = "myPath";
            string templateName = "myTemplate";
            var storedDataService = new StoredDataServiceMock() { ExistsTemplateReturn = false };
            var fileService = new FileServiceMock() { ExistsDirectoryReturn = true, ExistsTemplateConfigFileReturn = false };
            var commandDefinition = new AddTemplateCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                templatePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);

            Assert.Throws<TemplateConfigFileNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
         Trait("TestCategory", "CommandTest"),
         Trait("TestCategory", "DDCommandTest"),
         Trait("TestCategory", "AddTemplateCommandTest")]
        public void WhenExecuteCommandWithTemplateNonExistingPath_CommandManager_ShouldThrowException()
        {
            string templatePath = "myPath";
            string templateName = "myTemplate";
            var storedDataService = new StoredDataServiceMock() { ExistsTemplateReturn = false };
            var fileService = new FileServiceMock() { ExistsDirectoryReturn = false};
            var commandDefinition = new AddTemplateCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                templatePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);

            Assert.Throws<PathNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddTemplateCommandTest")]
        public void WhenExecuteCommandWithTemplateNameRepeated_CommandManager_ShouldThrowException()
        {
            string templatePath = "myPath";
            string templateName = "myTemplate";
            var storedDataService = new StoredDataServiceMock() { ExistsTemplateReturn = true };
            var fileService = new FileServiceMock();
            var commandDefinition = new AddTemplateCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                templatePath,
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);

            Assert.Throws<TemplateNameRepeatedException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
             Trait("TestCategory", "CommandTest"),
             Trait("TestCategory", "DDCommandTest"),
             Trait("TestCategory", "AddTemplateCommandTest")]
        public void WhenExecuteCommandWithoutNameParameter_CommandManager_ShouldThrowException()
        {
            string templatePath = "myPath";

            var storedDataService = new StoredDataServiceMock();
            var fileService = new FileServiceMock();
            var commandDefinition = new AddTemplateCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                templatePath);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddTemplateCommandTest")]
        public void WhenExecuteCommandWithoutPathParameter_CommandManager_ShouldThrowException()
        {
            string templateName = "myTemplate";

            var storedDataService = new StoredDataServiceMock();
            var fileService = new FileServiceMock();
            var commandDefinition = new AddTemplateCommand(storedDataService, fileService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }
    }
}
