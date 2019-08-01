using System;
using Xunit;

namespace DDCli.Test.Commands
{

    [Trait("TestCategory", "UnitTest"),
            Trait("TestCategory", "CommandTest"),
            Trait("TestCategory", "DevTest"),
            Trait("TestCategory", "DotNetTest")]
    public class OpenVisualStudioCommandTest
    {


        public OpenVisualStudioCommandTest()
        {

        }

        [Fact]
        public void Test1()
        {
            Assert.True(true);
        }
    }
}
