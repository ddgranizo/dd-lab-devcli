using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
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

    }
}
