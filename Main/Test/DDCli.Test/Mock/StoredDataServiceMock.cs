using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class StoredDataServiceMock : IStoredDataService
    {

        public string AddedCommand { get; set; }

        public string DeletedAlias { get; set; }

        public StoredDataServiceMock(bool returnBoolExistsAlias)
        {
            ReturnBoolExistsAlias = returnBoolExistsAlias;
        }

        public StoredDataServiceMock(List<string> aliasForReturn)
        {
            AliasForReturn = aliasForReturn ?? throw new ArgumentNullException(nameof(aliasForReturn));
        }


        public bool ReturnBoolExistsAlias { get; }
        public List<string> AliasForReturn { get; }

        public void DeleteAlias(string alias)
        {
            DeletedAlias = alias;
        }
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

        public List<string> GetAliasWithCommand()
        {
            return AliasForReturn;
        }

        public List<string> GetAlias()
        {
            throw new NotImplementedException();
        }
    }
}
