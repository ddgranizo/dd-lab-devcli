using System;
using Xunit;
using DDCli;
using DDCli.Models;
using DDCli.Test.Mock;
using DDCli.Commands;
using DDCli.Exceptions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DDCli.Interfaces;
using System.Linq;
using DDCli.Extensions;

namespace DDCli.Test
{

    public class CommandManagerTest
    {

        const string GenericCompleteCommandName = "MyCommandNameCommand";
        const string GenericCommandName = "MyCommandName";
        const string HelpCommandName = "Help";
        const string GenericNameSpace = "MyNamespace";
        const string GenericDescription = "MyDescription";
        const string GenericParameterName = "MyParameter";
        const string GenericParameterDescription = "MyParameterDescription";
        public string LastLog { get; set; }



        readonly IStoredDataService _storedDataServiceMock;
        readonly ICryptoService _cryptoServiceMock;
        readonly LoggerServiceMock _loggerServiceMock;
        public CommandManagerTest()
        {
            _storedDataServiceMock = new StoredDataServiceMock();
            _cryptoServiceMock = new CryptoServiceMock();
            _loggerServiceMock = new LoggerServiceMock();
        }

        private void Instance_OnLog(object sender, Events.LogEventArgs e)
        {
            _loggerServiceMock.Log(e.Log);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandInvalidGuidParameter_CommandManager_ShouldExecuteCommand()
        {
            var executedVerificationValue = Guid.Empty;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetGuidParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionGuid());



            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const string myInvalidGuidParamValue = "hellomoto";
            var inputRequest = GetGenericInputRequest(myInvalidGuidParamValue);

            Assert.Throws<InvalidCastException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandValidGuidParameter_CommandManager_ShouldExecuteCommand()
        {
            var executedVerificationValue = Guid.Empty;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetGuidParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionGuid());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            Guid myIntParamValue = Guid.NewGuid();
            var inputRequest = GetGenericInputRequest(myIntParamValue);

            instance.ExecuteInputRequest(inputRequest);

            var expected = myIntParamValue;
            var actual = executedVerificationValue;

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandInvalidDecimalParameter_CommandManager_ShouldExecuteCommand()
        {
            decimal executedVerificationValue = -1M;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetDecimalParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionDecimal());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const string myIntParamValue = "hellomoto";
            var inputRequest = GetGenericInputRequest(myIntParamValue);

            Assert.Throws<InvalidCastException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandValidDecimalParameter_CommandManager_ShouldExecuteCommand()
        {
            decimal executedVerificationValue = -1M;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetDecimalParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionDecimal());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const decimal myIntParamValue = 1.3M;
            var inputRequest = GetGenericInputRequest(myIntParamValue);

            instance.ExecuteInputRequest(inputRequest);

            var expected = myIntParamValue;
            var actual = executedVerificationValue;

            Assert.Equal(expected, actual);

        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandInvalidIntParameter_CommandManager_ShouldThrowCastException()
        {
            int executedVerificationValue = -1;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetIntParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionInteger());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const string myInvalidIntParamValue = "hellomoto";
            var inputRequest = GetGenericInputRequest(myInvalidIntParamValue);


            Assert.Throws<InvalidCastException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandValidIntParameter_CommandManager_ShouldExecuteCommand()
        {
            int executedVerificationValue = -1;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetIntParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionInteger());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const int myIntParamValue = 1;
            var inputRequest = GetGenericInputRequest(myIntParamValue);

            instance.ExecuteInputRequest(inputRequest);

            var expected = myIntParamValue;
            var actual = executedVerificationValue;

            Assert.Equal(expected, actual);

        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandValidAliasBoolParameter_CommandManager_ShouldExecuteCommand()
        {
            bool executedVerificationValue = false;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetBoolParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionBoolean());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const string myBoolParamValue = "yes";
            var inputRequest = GetGenericInputRequest(myBoolParamValue);

            instance.ExecuteInputRequest(inputRequest);

            var expected = true;
            bool actual = executedVerificationValue;

            Assert.Equal(expected, actual);

        }



        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandValidBoolParameter_CommandManager_ShouldExecuteCommand()
        {


            bool executedVerificationValue = false;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetBoolParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionBoolean());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const bool myBoolParamValue = true;
            var inputRequest = GetGenericInputRequest(myBoolParamValue);

            instance.ExecuteInputRequest(inputRequest);

            var expected = myBoolParamValue;
            bool actual = executedVerificationValue;

            Assert.Equal(expected, actual);

        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandValidStringParameter_CommandManager_ShouldExecuteCommand()
        {

            var executedVerificationValue = string.Empty;
            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.ExecuteAction = (inputParameters) =>
            {
                var parameterContent = commandDefinition.GetStringParameterValue(inputParameters, GenericParameterName.ToLowerInvariant());
                executedVerificationValue = parameterContent;
            };

            commandDefinition.CommandParametersDefinition.Add(GetGenericParameterDefinitionString());

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);

            const string myStringParamValue = "MyParamValue";
            var inputRequest = GetGenericInputRequest(myStringParamValue);

            instance.ExecuteInputRequest(inputRequest);

            var expected = myStringParamValue;
            var actual = executedVerificationValue;

            Assert.Equal(expected, actual);

        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecuteCommandWithNotValidParameters_CommandManager_ShouldThrowException()
        {

            var commandDefinition = GetCommandWithGenericParameter();
            commandDefinition.CommandParametersDefinition.Add(new CommandParameterDefinition(
                        GenericParameterName.ToLowerInvariant(),
                        CommandParameterDefinition.TypeValue.String,
                        GenericParameterDescription));

            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(commandDefinition);


            var inputRequest = new InputRequest(
                GenericCommandName.ToLowerInvariant(),
                $"--{GenericParameterName.ToLowerInvariant()}different",
                "MyParamValue");

            Assert.Throws<InvalidParamsException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });

        }



        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecutedCommandNotRegistered_CommandManager_ShouldThrowNotFoundException()
        {
            const string differentCommandName = "NotRegisteredCommand";
            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(new CommandMock(GenericNameSpace, differentCommandName, GenericDescription));
            var inputRequest = new InputRequest(GenericCommandName.ToLowerInvariant());

            Assert.Throws<CommandNotFoundException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }



        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecutedCommandWithSameNamespace_CommandManager_ShouldThrowDuplicateException()
        {
            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(new CommandMock($"{GenericNameSpace}1", GenericCompleteCommandName, GenericDescription));
            instance.RegisterCommand(new CommandMock($"{GenericNameSpace}2", GenericCompleteCommandName, GenericDescription));
            var inputRequest = new InputRequest(GenericCommandName.ToLowerInvariant());

            Assert.Throws<DuplicateCommandException>(() =>
            {
                instance.ExecuteInputRequest(inputRequest);
            });
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenExecutedCommandHelp_CommandManager_ShouldReturnHelpData()
        {
            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            instance.RegisterCommand(new CommandMock(GenericNameSpace, GenericCompleteCommandName, GenericDescription));
            instance.RegisterCommand(new HelpCommand(instance.Commands));
            var inputRequest = new InputRequest(HelpCommandName.ToLowerInvariant());

            instance.OnLog += Instance_OnLog;

            instance.ExecuteInputRequest(inputRequest);

            var expected = instance.Commands
                    .OrderBy(k => k.GetInvocationCommandName())
                    .ToDisplayList((item) => { return item.GetInvocationCommandName(); }, "Available commands:", "#");
            var actual = string.Join(Environment.NewLine, _loggerServiceMock.Logs.First().Split(Environment.NewLine).Skip(1).SkipLast(1));

            Assert.Equal(expected, actual);
        }

        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenRegisterInvalidCommand_CommandManager_ShouldThrowException()
        {
            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            const string commandName = "MyCommandName";
            var nameSpace = string.Empty;
            var description = string.Empty;

            Assert.Throws<ArgumentException>(() =>
            {
                instance.RegisterCommand(new CommandMock(nameSpace, commandName, description));
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenRegisterCommand_CommandManager_CommandShouldBeAddedWithinTheList()
        {
            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);

            instance.RegisterCommand(new CommandMock(GenericNameSpace, GenericCompleteCommandName, GenericDescription));

            var expected = GenericCompleteCommandName;
            var actual = instance.Commands[0].CommandName;

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenInstanceOfCommandManager_CommandManager_ShouldInitializeCommandsList()
        {
            var instance = new CommandManager(_loggerServiceMock, _storedDataServiceMock, _cryptoServiceMock);
            Assert.NotNull(instance.Commands);
        }


        private static CommandParameterDefinition GetGenericParameterDefinitionBoolean()
        {
            return new CommandParameterDefinition(
                                    GenericParameterName.ToLowerInvariant(),
                                    CommandParameterDefinition.TypeValue.Boolean,
                                    GenericParameterDescription);
        }
        private static CommandParameterDefinition GetGenericParameterDefinitionString()
        {
            return new CommandParameterDefinition(
                                    GenericParameterName.ToLowerInvariant(),
                                    CommandParameterDefinition.TypeValue.String,
                                    GenericParameterDescription);
        }

        private static CommandParameterDefinition GetGenericParameterDefinitionInteger()
        {
            return new CommandParameterDefinition(
                                    GenericParameterName.ToLowerInvariant(),
                                    CommandParameterDefinition.TypeValue.Integer,
                                    GenericParameterDescription);
        }

        private static CommandParameterDefinition GetGenericParameterDefinitionDecimal()
        {
            return new CommandParameterDefinition(
                                    GenericParameterName.ToLowerInvariant(),
                                    CommandParameterDefinition.TypeValue.Decimal,
                                    GenericParameterDescription);
        }

        private static CommandParameterDefinition GetGenericParameterDefinitionGuid()
        {
            return new CommandParameterDefinition(
                                    GenericParameterName.ToLowerInvariant(),
                                    CommandParameterDefinition.TypeValue.Guid,
                                    GenericParameterDescription);
        }

        private static CommandMock GetCommandWithGenericParameter()
        {
            var commandDefinition = new CommandMock(
                            GenericNameSpace,
                            GenericCompleteCommandName,
                            GenericDescription);
            commandDefinition.CanExecuteFunction = (inputParameters) =>
            {
                return commandDefinition.IsParamOk(inputParameters, GenericParameterName.ToLowerInvariant());
            };
            return commandDefinition;
        }


        private static InputRequest GetGenericInputRequest(object myInvalidIntParamValue)
        {
            return new InputRequest(
                            GenericCommandName.ToLowerInvariant(),
                            $"--{GenericParameterName.ToLowerInvariant()}",
                            myInvalidIntParamValue.ToString());
        }
    }
}
