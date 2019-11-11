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
    public class ShowAliasCommandTest
    {
        public string LastLog { get; set; }

        ICryptoService _cryptoServiceMock;
        IRegistryService _registryServiceMock;
        readonly LoggerServiceMock _loggerServiceMock;

        public ShowAliasCommandTest()
        {
            _cryptoServiceMock = new CryptoServiceMock();
            _registryServiceMock = new RegistryServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }

       


  

        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "ShowAliasCommandTest")]
        public void WhenExecuteCommandRegisteredAlias_CommandManager_ShouldReturnZeroAliasMessage()
        {

            string myAliasDefinition1 = "myalias => mycommand";
            string myAliasDefinition2 = "myalias2 => mycommand2";
            var aliasList = new List<string>() { myAliasDefinition1, myAliasDefinition2 };
            var storedDataService = new StoredDataServiceMock(aliasList);

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.OnLog += Instance_OnLog;
            var commandDefinition = new ShowAliasCommand(storedDataService);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = aliasList.ToDisplayList(
                ShowAliasCommand.HeaderForListingAlias, 
                ShowAliasCommand.FirstCharacterDefaultForListingAlias);
            var actual = _loggerServiceMock.Logs.First();

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DDCommandTest"),
            Trait("TestCategory", "ShowAliasCommandTest")]
        public void WhenExecuteCommandWithNoRegisteredAlias_CommandManager_ShouldReturnZeroAliasMessage()
        {
            var storedDataService = new StoredDataServiceMock(new List<string>());

            var instance = new CommandManager(_loggerServiceMock, storedDataService, _cryptoServiceMock);
            instance.OnLog += Instance_OnLog;
            var commandDefinition = new ShowAliasCommand(storedDataService);
            instance.RegisterCommand(commandDefinition);

            var inputRequest = new InputRequest(
                commandDefinition.GetInvocationCommandName());

            instance.ExecuteInputRequest(inputRequest);

            var expected = ShowAliasCommand.ZeroRegisteredMessage;
            var actual = _loggerServiceMock.Logs.First();

            Assert.Equal(expected, actual);
        }

        private void Instance_OnLog(object sender, Events.LogEventArgs e)
        {
            _loggerServiceMock.Log(e.Log);
        }
    }
}
