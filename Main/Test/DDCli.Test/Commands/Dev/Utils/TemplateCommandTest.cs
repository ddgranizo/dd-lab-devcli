using DDCli.Commands.Dev.Utils;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Test.Mock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DDCli.Test.Commands.Dev.Utils
{
    public class TemplateCommandTest
    {


        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        IConsoleService _consoleService;
        IStoredDataService _storedDataService;
        public TemplateCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _consoleService = new ConsoleServiceMock();
            _storedDataService = new StoredDataServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DevCommandTest"),
            Trait("TestCategory", "UtilsCommandTest"),
            Trait("TestCategory", "TemplateCommandTest")]
        public void WhenExecuteCommandWithValidTemplateConfig_CommandManager_ShouldThrowException()
        {
            var myAbsolutePath = @"c:\absolute\my\Path";
            var myPath = @"my\Path";
            var myTemplateName = "My template name";
            var myNewPathName = "MySecondApp";
            var myOldValue = "myOldValue";
            var myNewVale = "myNewValue";
            var fileService = new FileServiceMock()
            {
                DDTemplateConfigReturn = new DDTemplateConfig()
                {
                    TemplateName = myTemplateName,
                    IgnorePathPatterns = new List<string>(),
                    ReplacePairs = new List<ReplacePair>()
                    {
                        new ReplacePair()
                        {
                            ApplyForDirectories = true,
                            ApplyForFileContents = true,
                            ApplyForFileNames = true,
                            ApplyForFilePattern = "*.*",
                            OldValue = myOldValue,
                            ReplaceDescription = "My replace description"
                        }
                    }
                },
                ExistsTemplateConfigFileReturn = true,
                ExistsDirectoryReturn = true,
                AbsolutePathReturn = myAbsolutePath
            };
            var commandService = new ConsoleServiceMock()
            {
                ReadLineReturns = new List<string>()
                {
                    myNewPathName,
                    myNewVale
                }
            };
            var commandDefinition = new TemplateCommand(fileService, commandService);

            var instance = new CommandManager(_storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.PathParameter.GetInvokeName(),
                myPath);

            instance.ExecuteInputRequest(inputRequest);

            Assert.Equal(fileService.CreatedDirectory, myAbsolutePath);
            Assert.Equal(fileService.ClonedDirectorySource, myPath);
            Assert.Equal(fileService.ClonedDirectoryDestination, myAbsolutePath);
            Assert.Equal(fileService.ReplacedStringInPathsNewValue, myNewVale);
            Assert.Equal(fileService.ReplacedStringInPathsOldValue, myOldValue);
            Assert.Equal(fileService.ReplacedStringInPathsRootPath, myAbsolutePath);
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DevCommandTest"),
            Trait("TestCategory", "UtilsCommandTest"),
            Trait("TestCategory", "TemplateCommandTest")]
        public void WhenExecuteCommandWithNonExistingTemplateConfigFile_CommandManager_ShouldThrowException()
        {
            var myPath = @"my\Path";
            var fileService = new FileServiceMock() { ExistsTemplateConfigFileReturn = false, ExistsDirectoryReturn = true };
            var commandDefinition = new TemplateCommand(fileService, _consoleService);

            var instance = new CommandManager(_storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.PathParameter.GetInvokeName(),
                myPath);

            Assert.Throws<TemplateConfigFileNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DevCommandTest"),
            Trait("TestCategory", "UtilsCommandTest"),
            Trait("TestCategory", "TemplateCommandTest")]
        public void WhenExecuteCommandWithNonExistingFile_CommandManager_ShouldThrowException()
        {
            var myPath = @"my\Path";
            var fileService = new FileServiceMock() { ExistsDirectoryReturn = false };
            var commandDefinition = new TemplateCommand(fileService, _consoleService);

            var instance = new CommandManager(_storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.PathParameter.GetInvokeName(),
                myPath);

            Assert.Throws<PathNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DevCommandTest"),
            Trait("TestCategory", "UtilsCommandTest"),
            Trait("TestCategory", "TemplateCommandTest")]
        public void WhenExecuteCommandWithoutPathParameter_CommandManager_ShouldThrowException()
        {
            var fileService = new FileServiceMock();
            var commandDefinition = new TemplateCommand(fileService, _consoleService);

            var instance = new CommandManager(_storedDataService, _cryptoServiceMock);
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
