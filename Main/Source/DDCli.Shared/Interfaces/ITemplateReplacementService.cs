using System;
using System.Collections.Generic;
using System.Text;

namespace DDCli.Interfaces
{
    public interface ITemplateReplacementService
    {
        bool AskForConditional(IConsoleService consoleService, string iterationDescription);
        Dictionary<string, string> AskForInputParameters(IConsoleService consoleService, Dictionary<string, string> paramsDescription);
        List<Dictionary<string, string>> AskForIterationInputParameters(IConsoleService consoleService, string iterationDescription, Dictionary<string, string> paramsDescription);
        string Replace(string embebedResource, Dictionary<string, bool> conditionals, Dictionary<string, string> replacements, Dictionary<string, List<Dictionary<string, string>>> iterationReplacements);
        string Replace(string embebedResource, Dictionary<string, bool> conditionals, Dictionary<string, string> replacements);
        string Replace(string embebedResource, Dictionary<string, string> replacements);
    }
}
