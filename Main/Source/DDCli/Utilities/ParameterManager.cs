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
    public static class ParameterManager
    {
        private const string ParameterPattern = "\\[\\[[^\\]]+\\][\\+$ugaxvhd\\^]*\\]";

        public static List<CommandParameter> ResolveParameters(
            IStoredDataService storedDataService,
            List<CommandParameter> parameters)
        {
            var dictionaryKeyVaulues = GetDictionaryFromParameters(storedDataService);
            foreach (var parameter in parameters.Where(k => !string.IsNullOrEmpty(k.ValueString)))
            {
                parameter.ValueString = ReplaceStringParameters(dictionaryKeyVaulues, parameter.ValueString);
            }
            return parameters;
        }



        private static string ReplaceStringParameters(Dictionary<string, string> mappings, string rawString)
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
                    if (!mappings.ContainsKey(parameter))
                    {
                        throw new InvalidParamException(parameter);
                    }
                    replaced = replaced.Replace(parameter, mappings[parameter]);
                }
            } while (success);
            return replaced;
        }

        private static Dictionary<string, string> GetDictionaryFromParameters(IStoredDataService storedDataService)
        {
            var dictionaryKeyVaulues = new Dictionary<string, string>();
            storedDataService.GetParameters()
                .ForEach(k => { dictionaryKeyVaulues.Add($"[[{k}]]", storedDataService.GetParameterValue(k)); });
            return dictionaryKeyVaulues;
        }
    }
}
