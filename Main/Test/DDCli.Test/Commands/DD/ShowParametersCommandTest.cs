using DDCli.Commands.DD;
using DDCli.Exceptions;
using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Test.Mock;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;


namespace DDCli.Test.Commands.DD
{
    public class ShowParametersCommandTest
    {

        readonly ICryptoService _cryptoServiceMock;
        public string LastLog { get; set; }
        public ShowParametersCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "DDCommandTest"),
           Trait("TestCategory", "ShowParametersCommandTest")]
        public void WhenExecuteCommandWithZeroRegisteredParameters_CommandManager_ShouldShowZeroMessage()
        {
            var storedDataService = new StoredDataServiceMock()
            {
                ParametersWithValueForReturn = new List<string>()
            };
            var commandDefinition = new ShowParametersCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = ShowParametersCommand.ZeroRegisteredMessage;
            var actual = LastLog;

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "DDCommandTest"),
           Trait("TestCategory", "ShowParametersCommandTest")]
        public void WhenExecuteCommandWithRegisteredParameters_CommandManager_ShouldShowList()
        {
            var key1 = "my.pram1";
            var key2 = "my.pram2";
            var value1 = "my val 1";
            var value2 = "my val 2";

            var pair1 = $"{key1} => {value1}";
            var pair2 = $"{key2} => {value2}";

            var listed = new List<string>() { pair1, pair2 };

            var storedDataService = new StoredDataServiceMock()
            {
                ParametersWithValueForReturn = listed
            };
            var commandDefinition = new ShowParametersCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = listed.ToDisplayList(
                ShowParametersCommand.ParameterListHeaderDisplay,
                ShowParametersCommand.ParameterListFirstCharLine);
            var actual = LastLog;

            Assert.Equal(expected, actual);
        }

        private void Instance_OnLog(object sender, Events.LogEventArgs e)
        {
            LastLog = e.Log;
        }
    }
}
