using DDCli.Interfaces;
using DDCli.Models;
using DDCli.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Services
{
    public class StoredDataService : IStoredDataService
    {
        public StoredCliData StoredCliData { get; }
        public ICryptoService CryptoService { get; }

        public StoredDataService(StoredCliData storedCliData, ICryptoService cryptoService)
        {
            StoredCliData = storedCliData ?? throw new ArgumentNullException(nameof(storedCliData));
            CryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
        }


        public bool ExistsAlias(string alias)
        {
            return StoredCliData.CommandAlias.FirstOrDefault(k => k.Alias == alias) != null;
        }


        public void AddAlias(string command, string alias)
        {
            StoredCliData.CommandAlias.Add(new CommandAlias(command, alias));
            SaveContext();
        }

        public void DeleteAlias(string alias)
        {
            var aliasCommand =
                StoredCliData.CommandAlias.First(k => k.Alias == alias);
            StoredCliData.CommandAlias.Remove(aliasCommand);
            SaveContext();
        }

        private void SaveContext()
        {
            StoredDataManager.SaveStoredCliData(StoredCliData);
        }

        public List<string> GetAliasWithCommand()
        {
            return StoredCliData.CommandAlias.Select(k => string.Format("{0} => {1}", k.Alias, k.CommandName)).ToList();
        }

        public List<string> GetAlias()
        {
            return StoredCliData.CommandAlias.Select(k => k.Alias).ToList();
        }

        public string GetAliasedCommand(string alias)
        {
            return StoredCliData.CommandAlias.First(k => k.Alias == alias).CommandName;
        }

        public bool ExistsParameter(string parameter)
        {
            return StoredCliData.Parameters.FirstOrDefault(k => k.Key == parameter) != null;
        }

        public void AddParameter(string key, string value, bool isEncrypted)
        {
            StoredCliData.Parameters.Add(new CliParameter(key, isEncrypted ? CryptoService.Encrypt(value) : value , isEncrypted));
            SaveContext();
        }

        public void DeleteParameter(string key)
        {
            var parameterForRemove = StoredCliData.Parameters.First(k => k.Key == key);
            StoredCliData.Parameters.Remove(parameterForRemove);
            SaveContext();
        }

        public string GetParameterValue(string key)
        {
            var parameter = StoredCliData.Parameters.First(k => k.Key == key);
            return parameter.IsEncrypted ? CryptoService.Decrypt(parameter.Value) : parameter.Value;
        }

        public void UpdateParameter(string key, string newValue)
        {
            var parameterForUpdate = StoredCliData.Parameters.First(k => k.Key == key);
            parameterForUpdate.Value = parameterForUpdate.IsEncrypted ? CryptoService.Encrypt(newValue) : newValue;
            SaveContext();
        }

        public List<string> GetParametersWithValues()
        {
            return StoredCliData.Parameters
                .Select(k =>
                    string.Format("[[{0}]] => {1}", k.Key, k.IsEncrypted ? Definitions.PasswordOfuscator : k.Value))
                .ToList();
        }

        public List<CliParameter> GetParameters()
        {
            return StoredCliData.Parameters;
        }

        public void AddTemplate(string path, string templateName, string description)
        {
            StoredCliData.RegisteredTemplates.Add(new RegisteredTemplate(path, templateName, description));
            SaveContext();
        }

        public void DeleteTemplate(string templateName)
        {
            var templateForRemove = StoredCliData.RegisteredTemplates.First(k => k.TemplateName == templateName);
            StoredCliData.RegisteredTemplates.Remove(templateForRemove);
            SaveContext();
        }

        public List<string> GetTemplatesWithValues()
        {
            return StoredCliData.RegisteredTemplates
                .Select(k =>
                    string.Format("{0} => {1}, located at '{2}'", k.TemplateName, k.Description, k.Path))
                .ToList();
        }

        public List<RegisteredTemplate> GetTemplates()
        {
            return  StoredCliData.RegisteredTemplates.ToList();
        }

        public bool ExistsTemplate(string templateName)
        {
            return StoredCliData.RegisteredTemplates.Any(k => k.TemplateName == templateName);
        }

        public string GetTemplatePath(string templateName)
        {
            return StoredCliData.RegisteredTemplates.First(k => k.TemplateName == templateName).Path;
        }
    }
}
