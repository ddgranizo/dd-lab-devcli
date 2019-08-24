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
    public class MovePathCommandTest
    {
        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;


        public MovePathCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "WindowsTest"),
           Trait("TestCategory", "MovePathCommandTest")]
        public void WhenExecuteCommandWithFilePath_CommandManager_ShouldExecuteMoveFile()
        {
            var mySourcePath = "mypath";
            var myDestinationPath = "mypath";
            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock() { ExistsPathReturn = true, IsDirectoryReturn = false };
            var commandDefinition = new MovePathCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandSourcePathParameter.GetInvokeName(),
                mySourcePath,
                commandDefinition.CommandDestinationFolderParameter.GetInvokeName(),
                myDestinationPath);
            instance.ExecuteInputRequest(inputRequest);

            var expectedSourcePath = mySourcePath;
            var expectedDestionationPath = myDestinationPath;
            var actualSourcePath = fileServiceMock.MovedFileFrom;
            var actualDestinationPath = fileServiceMock.MovedFileTo;

            Assert.Equal(expectedSourcePath, actualSourcePath);
            Assert.Equal(expectedDestionationPath, actualDestinationPath);
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "WindowsTest"),
           Trait("TestCategory", "MovePathCommandTest")]
        public void WhenExecuteCommandWithDirectoryPath_CommandManager_ShouldExecuteMoveDirectoryContent()
        {
            var mySourcePath = "mypath";
            var myDestinationPath = "mypath";
            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock() { ExistsPathReturn = true, IsDirectoryReturn = true };
            var commandDefinition = new MovePathCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandSourcePathParameter.GetInvokeName(),
                mySourcePath,
                commandDefinition.CommandDestinationFolderParameter.GetInvokeName(),
                myDestinationPath);
            instance.ExecuteInputRequest(inputRequest);

            var expectedSourcePath = mySourcePath;
            var expectedDestionationPath = myDestinationPath;
            var actualSourcePath = fileServiceMock.MovedSourceFolder;
            var actualDestinationPath = fileServiceMock.MovedDestionationFolder;

            Assert.Equal(expectedSourcePath, actualSourcePath);
            Assert.Equal(expectedDestionationPath, actualDestinationPath);
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "MovePathCommandTest")]
        public void WhenExecuteCommandWithoutNonExistingSourcePath_CommandManager_ShouldThrowException()
        {
            var mySourcePath = "mypath";
            var myDestinationPath = "mypath";
            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock() { ExistsPathReturn = false };
            var commandDefinition = new MovePathCommand(fileServiceMock) ;
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandSourcePathParameter.GetInvokeName(),
                mySourcePath,
                commandDefinition.CommandDestinationFolderParameter.GetInvokeName(),
                myDestinationPath);

            Assert.Throws<PathNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }




        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "MovePathCommandTest")]
        public void WhenExecuteCommandWithoutDestinationPathParameter_CommandManager_ShouldThrowException()
        {
            var mySourcePath = "mypath";
            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new MovePathCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandSourcePathParameter.GetInvokeName(),
                mySourcePath);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "MovePathCommandTest")]
        public void WhenExecuteCommandWithoutSourcePathParameter_CommandManager_ShouldThrowException()
        {
            var myDestinationPath = "mypath";
            var storedDataService = new StoredDataServiceMock();

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new MovePathCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandDestinationFolderParameter.GetInvokeName(),
                myDestinationPath);

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

    }
}
