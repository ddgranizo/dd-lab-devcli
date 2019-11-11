using DDCli.Commands.DD;
using DDCli.Exceptions;
using DDCli.Extensions;
using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Test.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;


namespace DDCli.Test.Commands.DD
{
    public class ShowTemplatesCommandTest
    {

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        readonly LoggerServiceMock _loggerServiceMock;

        public string LastLog { get; set; }
        public ShowTemplatesCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "DDCommandTest"),
           Trait("TestCategory", "ShowTemplatesCommandTest")]
        public void WhenExecuteCommandWithZeroRegisteredParameters_CommandManager_ShouldShowZeroMessage()
        {
            var storedDataService = new StoredDataServiceMock()
            {
                TemplatesForReturn = new List<RegisteredTemplate>()
            };
            var commandDefinition = new ShowTemplatesCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = ShowTemplatesCommand.ZeroRegisteredMessage;
            var actual = _loggerServiceMock.Logs.First();

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"),
           Trait("TestCategory", "CommandTest"),
           Trait("TestCategory", "DDCommandTest"),
           Trait("TestCategory", "ShowTemplatesCommandTest")]
        public void WhenExecuteCommandWithRegisteredParameters_CommandManager_ShouldShowList()
        {
            var path1 = "my.path1";
            var path2 = "my.path2";
            var name1 = "myname1";
            var name2 = "myname2";

            var registeredTemplates = new List<RegisteredTemplate>()
            {
                new RegisteredTemplate(path1, name1, null),
                new RegisteredTemplate(path2, name2, null)
            };
            var storedDataService = new StoredDataServiceMock()
            {
                TemplatesForReturn = registeredTemplates
            };
            var commandDefinition = new ShowTemplatesCommand(storedDataService);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);
            instance.OnLog += Instance_OnLog;
            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = registeredTemplates.ToDisplayList(
                k => $"{k.TemplateName} => {k.Description} located at {k.Path}",
                ShowTemplatesCommand.ListHeaderDisplay,
                ShowTemplatesCommand.ListFirstCharLine);
            var actual = _loggerServiceMock.Logs.First();

            Assert.Equal(expected, actual);
        }

        private void Instance_OnLog(object sender, Events.LogEventArgs e)
        {
            _loggerServiceMock.Log(e.Log);
        }
    }
}
