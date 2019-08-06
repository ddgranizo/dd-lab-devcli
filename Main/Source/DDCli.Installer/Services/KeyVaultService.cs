using DDCli.Installer.Interfaces;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace DDCli.Installer.Services
{

    public class KeyVaultService : IKeyVaultService
    {
        public KeyVaultService() { }

        public string GetValueSecretFromKeyVault(string keyVaultName, string secretName)
        {
            if (keyVaultName == null)
            {
                throw new ArgumentNullException(nameof(keyVaultName));
            }

            if (secretName == null)
            {
                throw new ArgumentNullException(nameof(secretName));
            }

            var azureServiceTokenProvider = new AzureServiceTokenProvider();

            var keyVaultClient =
                new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

            return keyVaultClient.GetSecretAsync(keyVaultName, secretName).Result.Value;
        }

        public string ConvertToValidSecretName(string name)
        {
            var result = new Regex("[^0-9a-zA-Z-]", RegexOptions.Singleline).Replace(name, "-");

            return string.IsNullOrEmpty(result) ? "unknown" : result;
        }


    }
}
