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

    public delegate void OnReplacedParameterHandler(object sender, ReplacedParameterEventArgs args);
    public delegate void OnReplacedValueHandler(object sender, ReplacedParameterValueEventArgs args);
    public class ParameterManager
    {
        public event OnReplacedValueHandler OnReplacedEncrypted;
        public event OnReplacedParameterHandler OnReplacedAutoIncrement;

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
                parameter.ValueString = ReplaceSystemParameters(parameter.ValueString);
                parameter.ValueString = ReplaceStringParameters(storedParameters, parameter.ValueString);
            }
            
            return parameters;
        }


        private string ReplaceSystemParameters(string rawString)
        {
            rawString = rawString.Replace("[[$now]]", DateTime.Now.ToString());
            return rawString;
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
                        OnReplacedEncrypted?.Invoke(this, new ReplacedParameterValueEventArgs(toReplaceParameterValue));
                    }
                    if (storedParameter.IsAutoIncrement)
                    {
                        OnReplacedAutoIncrement?.Invoke(this, new ReplacedParameterEventArgs(storedParameter.Key));
                    }
                }
            } while (success);
            return replaced;
        }


    }
}
