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

        public Dictionary<string, List<Dictionary<string, string>>> ReturnIterationParameters { get; set; }
        public Dictionary<string, List<Dictionary<string, string>>> AskForIterationInputParameters(IConsoleService consoleService, string iterationDescription, string iterationIdentifier, Dictionary<string, string> paramsDescription)
        {
            return ReturnIterationParameters;
        }

        public List<string> ReturnedContents { get; set; }
        public int ReturnedContentCounter { get; set; } 
        public string Replace(string embebedResource, Dictionary<string, string> replacements, Dictionary<string, List<Dictionary<string, string>>> iterationReplacements)
        {
            return ReturnedContents[ReturnedContentCounter++];
        }
    }
}
