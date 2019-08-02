using System;
using Xunit;
using DDCli;
using DDCli.Models;
using Moq;
using DDCli.Test.Mock;

namespace DDCli.Test
{

    [Trait("TestCategory", "UnitTest"), Trait("TestCategory", "CommandManagerTest")]
    public class CommandManagerTest
    {

        public CommandManagerTest()
        {

        }



        [Fact]
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
        public void WhenInstanceOfCommandManager_CommandManager_ShouldInitializeCommandsList()
        {
            var instance = new CommandManager();
            Assert.NotNull(instance.Commands);
        }
    }
}
