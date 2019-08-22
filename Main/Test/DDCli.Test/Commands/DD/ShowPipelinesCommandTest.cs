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
    public class ShowPipelinesCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        public string LastLog { get; set; }
        public ShowPipelinesCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "DDCommandTest"),
           Trait("TestCategory", "ShowPipelinesCommandTest")]
        public void WhenExecuteCommandWithZeroRegisteredParameters_CommandManager_ShouldShowZeroMessage()
        {
            var storedDataService = new StoredDataServiceMock()
            {
                GetPipelinesReturn = new List<RegisteredPipeline>()
            };
            var commandDefinition = new ShowPipelinesCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = ShowPipelinesCommand.ZeroRegisteredMessage;
            var actual = LastLog;

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "DDCommandTest"),
           Trait("TestCategory", "ShowPipelinesCommandTest")]
        public void WhenExecuteCommandWithRegisteredParameters_CommandManager_ShouldShowList()
        {
            var path1 = "my.path1";
            var path2 = "my.path2";
            var name1 = "myname1";
            var name2 = "myname2";

            var registeredPipelines = new List<RegisteredPipeline>()
            {
                new RegisteredPipeline(path1, name1, null),
                new RegisteredPipeline(path2, name2, null)
            };
            var storedDataService = new StoredDataServiceMock()
            {
                GetPipelinesReturn = registeredPipelines
            };
            var commandDefinition = new ShowPipelinesCommand(storedDataService);

            var instance = new CommandManager(storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = registeredPipelines.ToDisplayList(
                k => $"{k.PipelineName} => {k.Description} located at {k.Path}",
                ShowPipelinesCommand.ListHeaderDisplay,
                ShowPipelinesCommand.ListFirstCharLine);
            var actual = LastLog;

            Assert.Equal(expected, actual);
        }

        private void Instance_OnLog(object sender, Events.LogEventArgs e)
        {
            LastLog = e.Log;
        }
    }
}
