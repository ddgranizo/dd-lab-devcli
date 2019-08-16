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
    public class RenameFolderCommandTest
    {
        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;


        public RenameFolderCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "WindowsTest"),
           Trait("TestCategory", "RenameFolderCommandTest")]
        public void WhenExecuteCommandWithValidFile_CommandManager_ShouldZipDirectory()
        {
            var path = "myPath";
            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock() { IsDirectoryReturn = false };
            var commandDefinition = new ZipCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                path);

            instance.ExecuteInputRequest(inputRequest);

            var expected = path;
            var actual = fileServiceMock.ZippedPath;

            Assert.Equal(expected, actual);
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "WindowsTest"),
           Trait("TestCategory", "RenameFolderCommandTest")]
        public void WhenExecuteCommandWithValidPaths_CommandManager_ShouldRename()
        {
            var newPath = "myNewPath";
            var oldPath = "myOldPath";
            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new RenameFolderCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathOldFolderParameter.GetInvokeName(),
                oldPath,
                commandDefinition.CommandPathNewFolderParameter.GetInvokeName(),
                newPath);

            instance.ExecuteInputRequest(inputRequest);

            var expectedOldPath = oldPath;
            var expectedONewPath = newPath;
            var actualOldPath = fileServiceMock.RenamedOldFolder;
            var actualNewPath = fileServiceMock.RenamedNewFolder;
            Assert.Equal(expectedOldPath, actualOldPath);
            Assert.Equal(expectedONewPath, actualNewPath);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "WindowsTest"),
           Trait("TestCategory", "RenameFolderCommandTest")]
        public void WhenExecuteCommandWithoutOldPathParameter_CommandManager_ShouldThrowException()
        {
            var newPath = "myNewPath";
            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new RenameFolderCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathNewFolderParameter.GetInvokeName(),
                newPath);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "RenameFolderCommandTest")]
        public void WhenExecuteCommandWithoutNewPathParameter_CommandManager_ShouldThrowException()
        {
            var oldPath = "myOldPath";
            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new RenameFolderCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathOldFolderParameter.GetInvokeName(),
                oldPath);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

    }
}
