using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class StoredDataServiceMock : IStoredDataService
    {

        public string AddedCommand { get; set; }

        public StoredDataServiceMock(bool returnBoolExistsAlias)
        {
            ReturnBoolExistsAlias = returnBoolExistsAlias;
        }

        public bool ReturnBoolExistsAlias { get; }

        public void AddAlias(string command, string alias)
        {
            AddedCommand = command;
        }

        public bool ExistsAlias(string alias)
        {
            return ReturnBoolExistsAlias;
        }

        public string GetStoredKey()
        {
            return null;
        }
    }
}
