using System;
using Xunit;
using DDCli;
using DDCli.Models;
using Moq;
using DDCli.Test.Mock;

namespace DDCli.Test
{

    public class CommandManagerTest
    {

        public CommandManagerTest()
        {

        }

        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenRegisterInvalidCommand_CommandManager_ShouldThrowException()
        {
            var instance = new CommandManager();
            const string commandName = "MyCommandName";
            var nameSpace = string.Empty;
            var description = string.Empty;

            Assert.Throws<ArgumentException>(() =>
            {
                instance.RegisterCommand(new MockCommand(nameSpace, commandName, description));
            });
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenRegisterCommand_CommandManager_CommandShouldBeAddedWithinTheList()
        {
            var instance = new CommandManager();
            const string commandName = "MyCommandNameCommand";
            const string nameSpace = "MyNamespace";
            const string description = "MyDescription";
            instance.RegisterCommand(new MockCommand(nameSpace, commandName, description));

            var expected = commandName;
            var actual = instance.Commands[0].CommandName;

            Assert.Equal(expected, actual);
        }


        [Fact]
        [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
        public void WhenInstanceOfCommandManager_CommandManager_ShouldInitializeCommandsList()
        {
            var instance = new CommandManager();
            Assert.NotNull(instance.Commands);
        }
    }
}
