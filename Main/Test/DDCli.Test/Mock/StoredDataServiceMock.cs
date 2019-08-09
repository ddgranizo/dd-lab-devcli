using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;

namespace DDCli.Test.Mock
{
    public class StoredDataServiceMock : IStoredDataService
    {

        public string AddedCommand { get; set; }

        public string DeletedAlias { get; set; }



        public bool ReturnBoolExistsAlias { get; }
        public List<string> AliasForReturn { get; }

        public string AddedParameterKey { get; set; }

        public string AddedParameterValue { get; set; }

        public bool ReturnBoolExistsParameter { get; set; }

        public List<string> ParametersWithValueForReturn { get; set; }

        public string DeletedParameter { get; set; }

        public string UpdatedParameterKey { get; set; }
        public string UpdatedParameterValue { get; set; }

        public StoredDataServiceMock()
        {

        }
        public StoredDataServiceMock(bool returnBoolExistsAlias)
        {
            ReturnBoolExistsAlias = returnBoolExistsAlias;
        }

        public StoredDataServiceMock(List<string> aliasForReturn)
        {
            AliasForReturn = aliasForReturn ?? throw new ArgumentNullException(nameof(aliasForReturn));
        }


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

        public string GetAliasedCommand(string alias)
        {
            throw new NotImplementedException();
        }

        public bool ExistsParameter(string parameter)
        {
            return ReturnBoolExistsParameter;
        }

        public void AddParameter(string key, string value, bool isEncrypted)
        {
            AddedParameterKey = key;
            AddedParameterValue = value;
        }

        public void DeleteParameter(string key)
        {
            DeletedParameter = key;
        }

        public string GetParameterValue(string key)
        {
            return string.Empty;
        }

        public void UpdateParameter(string key, string newValue)
        {
            UpdatedParameterValue = newValue;
            UpdatedParameterKey = key;
        }

        public List<string> GetParametersWithValues()
        {
            return ParametersWithValueForReturn;
        }


        public List<CliParameter> GetParameters()
        {
            return new List<CliParameter>();
        }

    }
}
