using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Interfaces
{
    public interface ITemplateReplacementService
    {

        Dictionary<string, string> AskForInputParameters(IConsoleService consoleService, Dictionary<string, string> paramsDescription);
        Dictionary<string, List<Dictionary<string, string>>> AskForIterationInputParameters(IConsoleService consoleService, string iterationDescription, string iterationIdentifier, Dictionary<string, string> paramsDescription);
        string Replace(string embebedResource, Dictionary<string, string> replacements, Dictionary<string, List<Dictionary<string, string>>> iterationReplacements);
    }
}
