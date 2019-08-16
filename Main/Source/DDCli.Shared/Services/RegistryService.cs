using DDCli.Interfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Services
{
    public class RegistryService : IRegistryService
    {
        private const string RegistryPath = @"SOFTWARE\DDCliSettings";
        private readonly RegistryKey _registryKey;

        
        public RegistryService()
        {
            _registryKey = Registry.CurrentUser.CreateSubKey(RegistryPath);
        }


        public string GetValue(string key)
        {
            return _registryKey.GetValue(key)?.ToString();
        }

        public void SetValue(string key, string value)
        {
            _registryKey.SetValue(key, value.ToString());
        }
    }
}
