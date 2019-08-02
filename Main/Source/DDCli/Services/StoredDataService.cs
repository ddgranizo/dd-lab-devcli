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
        public StoredDataService(StoredCliData storedCliData)
        {
            StoredCliData = storedCliData ?? throw new ArgumentNullException(nameof(storedCliData));
        }


        public string GetStoredKey()
        {
            return string.Empty;
        }

        public bool ExistsAlias(string alias)
        {
            return StoredCliData.CommandAlias.FirstOrDefault(k => k.Alias == alias) != null;
        }


        public void AddAlias(string command, string alias)
        {
            StoredCliData.CommandAlias.Add(new CommandAlias(command, alias));
        }


        private void SaveContext()
        {
            StoredDataManager.SaveStoredCliData(StoredCliData);
        }
    }
}
