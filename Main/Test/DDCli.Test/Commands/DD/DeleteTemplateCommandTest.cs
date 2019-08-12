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
    public class DeleteTemplateCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;


        public DeleteTemplateCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
                 Trait("TestCategory", "CommandTest"),
                 Trait("TestCategory", "DDCommandTest"),
                 Trait("TestCategory", "DeleteTemplateCommandTest")]
        public void WhenExecuteCommandWithValidTemplate_CommandManager_ShouldThrowException()
        {
            string templateName = "myTemplate";
            var storedDataService = new StoredDataServiceMock() { ExistsTemplateReturn = true };
            var commandDefinition = new DeleteTemplateCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);
            
            instance.ExecuteInputRequest(inputRequest);

            var expected = templateName;
            var actual = storedDataService.DeletedTemplate;

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
         Trait("TestCategory", "CommandTest"),
         Trait("TestCategory", "DDCommandTest"),
         Trait("TestCategory", "DeleteTemplateCommandTest")]
        public void WhenExecuteCommandWithTemplateNonExistingTemplate_CommandManager_ShouldThrowException()
        {
            string templateName = "myTemplate";
            var storedDataService = new StoredDataServiceMock() { ExistsTemplateReturn = false };
            var commandDefinition = new DeleteTemplateCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNameParameter.GetInvokeName(),
                templateName);

            Assert.Throws<TemplateNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "DeleteTemplateCommandTest")]
        public void WhenExecuteCommandWithoutNameParameter_CommandManager_ShouldThrowException()
        {

            var storedDataService = new StoredDataServiceMock();
            var commandDefinition = new DeleteTemplateCommand(storedDataService);

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
