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
    public class UnzipCommandTest
    {
        readonly ICryptoService _cryptoServiceMock;


        public UnzipCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "WindowsTest"),
           Trait("TestCategory", "UnzipCommandTest")]
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
           Trait("TestCategory", "UnzipCommandTest")]
        public void WhenExecuteCommandWithValidFolder_CommandManager_ShouldZipDirectory()
        {
            var path = "myPath";
            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock() { IsFileReturn = true};
            var commandDefinition = new UnzipCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                path);

            instance.ExecuteInputRequest(inputRequest);

            var expected = path;
            var actual = fileServiceMock.UnzippedPath;

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "WindowsTest"),
           Trait("TestCategory", "UnzipCommandTest")]
        public void WhenExecuteCommandWithInvalidFile_CommandManager_ShouldThrowException()
        {
            var path = "myPath";
            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock() { IsFileReturn = false };
            var commandDefinition = new UnzipCommand(fileServiceMock);
            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandPathParameter.GetInvokeName(),
                path);

            Assert.Throws<InvalidZipFileException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "WindowsTest"),
            Trait("TestCategory", "UnzipCommandTest")]
        public void WhenExecuteCommandWithoutPathParameter_CommandManager_ShouldThrowException()
        {

            var storedDataService = new StoredDataServiceMock(false);

            var fileServiceMock = new FileServiceMock();
            var commandDefinition = new UnzipCommand(fileServiceMock);
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
