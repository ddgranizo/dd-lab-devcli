using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class RegistryServiceMock : IRegistryService
    {
        public RegistryServiceMock()
        {
        }

        public string GetValue(string key)
        {
            throw new NotImplementedException();
        }

        public void SetValue(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
