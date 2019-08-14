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
    public class WindowCmdCommandTest
    {
        


        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        public string LastLog { get; set; }
        public WindowCmdCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "WindowCmdCommandTest")]
        public void WhenExecuteCommandCmd_CommandManager_ShouldExecuteCommand()
        {
            var cmd = "mycommand";
            var storedDataService = new StoredDataServiceMock();

            var promtpService = new PromptServiceMock();

            var commandDefinition = new WindowsCmdCommand(promtpService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName(),
                commandDefinition.CommandCmdParameter.GetInvokeName(),
                cmd);
            instance.ExecuteInputRequest(inputRequest);

            var expected = cmd;
            var actual = promtpService.RunCommandValue;

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "WindowCmdCommandTest")]
        public void WhenExecuteCommandWithoutCmd_CommandManager_ShouldThrowException()
        {

            var storedDataService = new StoredDataServiceMock();

            var promtpService = new PromptServiceMock();

            var commandDefinition = new WindowsCmdCommand(promtpService);

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
