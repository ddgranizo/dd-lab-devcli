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


        public string AddedTemplateName { get; set; }
        public string AddedTemplatePath { get; set; }

        public string DeletedTemplate { get; set; }

        public List<string> TemplatesDisplayForReturn { get; set; }

        public List<RegisteredTemplate> TemplatesForReturn { get; set; }

        public bool ExistsTemplateReturn { get; set; }

        public string GetTemplatePathReturn { get; set; }

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


        public void AddParameter(string key, string value, bool isEncrypted, bool isAutoIncrement)
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

        public void AddTemplate(string path, string templateName, string description)
        {
            AddedTemplateName = templateName;
            AddedTemplatePath = path;
        }

        public void DeleteTemplate(string templateName)
        {
            DeletedTemplate = templateName;
        }

        public List<string> GetTemplatesWithValues()
        {
            return TemplatesDisplayForReturn;
        }

        public List<RegisteredTemplate> GetTemplates()
        {
            return TemplatesForReturn;
        }

        public bool ExistsTemplate(string templateName)
        {
            return ExistsTemplateReturn;
        }

        public string GetTemplatePath(string templateName)
        {
            return GetTemplatePathReturn;
        }


        public void UpdateAutoIncrements(List<string> autoincrementParameters)
        {
            
        }
    }
}
