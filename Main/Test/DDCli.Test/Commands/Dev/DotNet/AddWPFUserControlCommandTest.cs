using DDCli.Commands.DD;
using DDCli.Commands.Dev.DotNet;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Services;
using DDCli.Test.Mock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace DDCli.Test.Commands.DD
{
    public class AddWPFUserControlCommandTest
    {

        readonly ICryptoService _cryptoServiceMock;


        public AddWPFUserControlCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
        }

   

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "AddAliasCommandTest")]
        public void WhenExecuteCommand_AddWPFControlCommand_ShouldGenerateFiles()
        {
            var controllerReplaced1 = "ControllerPathContent";
            var viewModelReplaced2 = "ViewModelPathContent";
            var ViewReplaced3 = "ViewPathContent";
            var className = "MyClass";

            var storedDataService = new StoredDataServiceMock(false);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);

            var fileService = new FileServiceMock() ;

            var replacementParameter = new Dictionary<string, string>();
            replacementParameter["ClassName"] = className;


            var templateService = new TemplateReplacementServiceMock()
            {
                ReturnParameters = replacementParameter,
                ReturnedContents = new List<string>()
                {
                    controllerReplaced1,
                    viewModelReplaced2,
                    ViewReplaced3
                }
            };
            var commandDefinition = new AddWPFUserControlCommand(fileService, templateService);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expectedPath1 = controllerReplaced1;
            var actualPath1 = fileService.FilesWritten[0];

            var expectedPath2 = viewModelReplaced2;
            var actualPath2 = fileService.FilesWritten[1];

            var expectedPath3 = ViewReplaced3;
            var actualPath3 = fileService.FilesWritten[2];


            Assert.Equal(expectedPath1, actualPath1);
            Assert.Equal(expectedPath2, actualPath2);
            Assert.Equal(expectedPath3, actualPath3);
        }


    }
}
