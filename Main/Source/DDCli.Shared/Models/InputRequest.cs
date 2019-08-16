using DDCli.Exceptions;
using DDCli.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Models
{
    public class InputRequest
    {
        public string CommandName { get; set; }
        public string CommandNamespace { get; set; }

        public List<InputParameter> InputParameters { get; set; }
        public InputRequest(params string[] args)
        {
            InputParameters = new List<InputParameter>();
            if (args.Length == 0)
            {
                throw new NotArgumentsException();
            }
            var commandRawName = args[0].ToLowerInvariant();
            if (!IsCommandNameValid(commandRawName))
            {
                throw new NotValidCommandNameException(commandRawName);
            }
            CommandName = GetCommandName(commandRawName);
            CommandNamespace = GetNameSpace(commandRawName);

            var parametersArr = args.Skip(1).ToArray();
            if (parametersArr.ToList().Count > 0)
            {
                InputParameters = GetInputParameters(parametersArr);
            }
        }

        private List<InputParameter> GetInputParameters(string[] parameterArr)
        {
            for (int i = 0; i < parameterArr.Length; i++)
            {
                Console.WriteLine(i + "- " + parameterArr[i]);
            }
            var parameters = new List<InputParameter>();
            if (parameterArr.Length == 1 && !IsParamNameValid(parameterArr[0]))
            {
                parameters.Add(new InputParameter( parameterArr[0]) );
            }
            else
            {
                bool nextIsValue = false;
                for (int i = 0; i < parameterArr.Length; i++)
                {
                    var parameter = parameterArr[i];
                    if (!nextIsValue)
                    {
                        bool isValidParam = IsParamNameValid(parameter);
                        bool isValidShortCut = isValidParam
                            ? false :
                            IsParamShortCutValid(parameter);
                        if (!isValidParam && !isValidShortCut)
                        {
                            throw new InvalidParamNameException(parameter);
                        }
                        string paramValue = null;
                        string parameterTrimmed = isValidShortCut
                                ? parameter.Substring("-".Length)
                                : parameter.Substring("--".Length);
                        if (i < parameterArr.Length - 1
                                && !IsParamNameValid(parameterArr[i + 1])
                                && !IsParamShortCutValid(parameterArr[i + 1]))
                        {
                            paramValue = parameterArr[i + 1];
                            nextIsValue = true;
                        }
                        parameters.Add(new InputParameter(parameterTrimmed, paramValue, isValidShortCut));
                    }
                    else
                    {
                        nextIsValue = false;
                    }
                }
            }
            return parameters;
        }


        private string GetNameSpace(string commandRawName)
        {
            return string.Join(".", commandRawName.Split('-').SkipLastCustom(1).ToArray());
        }

        private string GetCommandName(string commandRawName)
        {
            return string.Format("{0}command", commandRawName.Split('-').ToList().Last());
        }

        private bool IsParamShortCutValid(string parameter)
        {
            return !string.IsNullOrEmpty(parameter) && parameter.Length > 1 && parameter.Substring(0, 1) == "-";
        }

        private bool IsParamNameValid(string parameter)
        {
            return !string.IsNullOrEmpty(parameter) && parameter.Length > 2 && parameter.Substring(0, 2) == "--";
        }

        private bool IsCommandNameValid(string parameter)
        {
            return !string.IsNullOrEmpty(parameter) && parameter.Length > 2 && Char.IsLetter(parameter[0]) && Char.IsLetter(parameter[1]);
        }
    }
}
