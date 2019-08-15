using DDCli.Commands.Windows;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Test.Mock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DDCli.Test.Commands.Windows
{
    public class ReplaceTextCommandTest
    {
        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;


        public ReplaceTextCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "ReplaceTextCommandTest")]
        public void WhenExecuteCommandWithValidParams_CommandManager_ShouldExecuteReplace()
        {
            var newValue = "myNewValue";
            var oldValue = "myOldValue";
            var path = "myPath";
            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new ReplaceFileContentCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNewValueParameter.GetInvokeName(),
                newValue,
                commandDefinition.CommandOldValueParameter.GetInvokeName(),
                oldValue,
                commandDefinition.CommandPathParameter.GetInvokeName(),
                path);

            instance.ExecuteInputRequest(inputRequest);

            var expectedPath = path;
            var expectedOldValue = oldValue;
            var expectedNewValue = newValue;

            var actualPath = fileServiceMock.ReplacedFilesContentsPath;
            var actualOldValue = fileServiceMock.ReplacedStringInPathsOldValue;
            var actualNewValue = fileServiceMock.ReplacedStringInPathsNewValue;

            Assert.Equal(expectedPath, actualPath);
            Assert.Equal(expectedOldValue, actualOldValue);
            Assert.Equal(expectedNewValue, actualNewValue);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "ReplaceTextCommandTest")]
        public void WhenExecuteCommandWithoutPathParameter_CommandManager_ShouldThrowException()
        {
            var newValue = "myNewValue";
            var oldValue = "myOldValue";
            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new ReplaceFileContentCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandNewValueParameter.GetInvokeName(),
                newValue,
                commandDefinition.CommandOldValueParameter.GetInvokeName(),
                oldValue);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });

        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "ReplaceTextCommandTest")]
        public void WhenExecuteCommandWithoutOldValueParameter_CommandManager_ShouldThrowException()
        {
            var path = "mypath";
            var newValue = "myOldValue";

            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new ReplaceFileContentCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                path,
                commandDefinition.CommandNewValueParameter.GetInvokeName(),
                newValue);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "ReplaceTextCommandTest")]
        public void WhenExecuteCommandWithoutNewValueParameter_CommandManager_ShouldThrowException()
        {
            var path = "mypath";
            var oldValue = "myOldValue";
            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new ReplaceFileContentCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                path,
                commandDefinition.CommandOldValueParameter.GetInvokeName(),
                oldValue);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

    }
}
