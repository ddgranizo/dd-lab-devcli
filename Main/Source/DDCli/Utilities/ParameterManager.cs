using DDCli.Events;
using DDCli.Exceptions;
using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DDCli.Utilities
{

    public delegate void OnReplacedEncryptedHandler(object sender, ReplacedEncryptedEventArgs args);
    public class ParameterManager
    {
        public event OnReplacedEncryptedHandler OnReplacedEncrypted; //TODO MAÑANA: cada vez que se reemplace un encriptado, avisar al command manager para que cualquier Log lo obfusque

        private const string ParameterPattern = "\\[\\[[^\\]]+\\][\\+$ugaxvhd\\^]*\\]";

        public ICryptoService CryptoService { get; }

        public ParameterManager(ICryptoService cryptoService)
        {
            CryptoService = cryptoService ?? throw new ArgumentNullException(nameof(cryptoService));
        }

        public List<CommandParameter> ResolveParameters(
            IStoredDataService storedDataService,
            List<CommandParameter> parameters)
        {
            var storedParameters = storedDataService.GetParameters();
            foreach (var parameter in parameters.Where(k => !string.IsNullOrEmpty(k.ValueString)))
            {
                parameter.ValueString = ReplaceStringParameters(storedParameters, parameter.ValueString);
            }
            return parameters;
        }



        private string ReplaceStringParameters(List<CliParameter> storedParameters, string rawString)
        {
            var replaced = rawString;
            var regex = new Regex(ParameterPattern, RegexOptions.Compiled);
            bool success = false;
            do
            {
                var match = regex.Match(replaced);
                success = match.Success;
                if (success)
                {
                    var parameter = match.Groups[0].Value;
                    var storedParameter = storedParameters.FirstOrDefault(k => $"[[{k.Key}]]" == parameter);
                    if (storedParameter == null)
                    {
                        throw new InvalidParamException(parameter);
                    }
                    var toReplaceParameterValue = storedParameter.IsEncrypted ? CryptoService.Decrypt(storedParameter.Value) : storedParameter.Value;
                    replaced = replaced.Replace(parameter, toReplaceParameterValue);
                    if (storedParameter.IsEncrypted)
                    {
                        OnReplacedEncrypted?.Invoke(this, new ReplacedEncryptedEventArgs(toReplaceParameterValue));
                    }
                }
            } while (success);
            return replaced;
        }


    }
}
