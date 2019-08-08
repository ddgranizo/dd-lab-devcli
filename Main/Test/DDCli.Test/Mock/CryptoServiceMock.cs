using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class CryptoServiceMock : ICryptoService
    {
        public CryptoServiceMock()
        {
        }

        public string Decrypt(string str)
        {
            throw new NotImplementedException();
        }

        public string Encrypt(string str)
        {
            throw new NotImplementedException();
        }
    }
}
