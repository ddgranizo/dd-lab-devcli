using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Test.Mock
{
    public class TemplateReplacementServiceMock : ITemplateReplacementService
    {

        public Dictionary<string, string> ReturnParameters { get; set; }
        public TemplateReplacementServiceMock()
        {
            
        }

        public Dictionary<string, string> AskForInputParameters(IConsoleService consoleService, Dictionary<string, string> paramsDescription)
        {
            return ReturnParameters;
        }


        public List<Dictionary<string, string>> ReturnIterationParameters { get; set; }
        public List<Dictionary<string, string>> AskForIterationInputParameters(IConsoleService consoleService, string iterationDescription, Dictionary<string, string> paramsDescription)
        {
            return ReturnIterationParameters;
        }

        public List<string> ReturnedContents { get; set; }
        public int ReturnedContentCounter { get; set; }
        public string Replace(string embebedResource, Dictionary<string, bool> conditionals, Dictionary<string, string> replacements, Dictionary<string, List<Dictionary<string, string>>> iterationReplacements)
        {
            return ReturnedContents[ReturnedContentCounter++];
        }

        public bool ReturnedConditional { get; set; }
        public bool AskForConditional(IConsoleService consoleService, string iterationDescription)
        {
            return ReturnedConditional;
        }

        public string Replace(string embebedResource, Dictionary<string, bool> conditionals, Dictionary<string, string> replacements)
        {
            throw new NotImplementedException();
        }

        public string Replace(string embebedResource, Dictionary<string, string> replacements)
        {
            throw new NotImplementedException();
        }
    }
}
