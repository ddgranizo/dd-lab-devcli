using System;
using Xunit;

namespace DDCli.Test.Commands
{

    
    public class CommandManagerTest
    {
        public CommandManagerTest()
        {

        }

        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DevTest"),
            Trait("TestCategory", "DotNetTest")]
        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }


        [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DevTest"),
            Trait("TestCategory", "DotNetTest")]
        [Fact]
        public void Test2()
        {
            Assert.True(true);
        }
    }
}
