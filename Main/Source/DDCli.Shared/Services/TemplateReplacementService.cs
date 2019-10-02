using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace DDCli.Services
{
    public class TemplateReplacementService : ITemplateReplacementService
    {
        private const string _iterationPattern = "\\${0}\\$(.*?)\\$\\$";

        public TemplateReplacementService()
        {
        }

        public string Replace(string embebedResource, Dictionary<string, string> replacements, Dictionary<string, List<Dictionary<string, string>>> iterationReplacements)
        {
            var templateContent = LoadResource($"{embebedResource}.txt");
            foreach (var item in replacements)
            {
                templateContent = templateContent.Replace($"[[{item.Key}]]", item.Value);
            }

            foreach (var genericIterations in iterationReplacements)
            {
                var iterationIdentifier = genericIterations.Key;
                var parametersListForReplace = genericIterations.Value;
                var currentPattern = string.Format(_iterationPattern, iterationIdentifier);
                var iterations = Regex.Matches(templateContent, currentPattern, RegexOptions.Singleline)
                       .Cast<Match>();

                foreach (var iterationModel in iterations)
                {
                    var completedIterationModel = iterationModel.Groups[0].Value;
                    var innerIterationModelValue = iterationModel.Groups[1].Value;
                    var replacedIteration = new StringBuilder();

                    foreach (var listItem in parametersListForReplace)
                    {
                        var listItemModel = innerIterationModelValue;
                        var replacedItemList = new StringBuilder();
                        foreach (var parameter in listItem.Keys)
                        {
                            listItemModel = listItemModel.Replace($"[[{iterationIdentifier}.{parameter}]]", listItem[parameter]);
                        }
                        replacedIteration.AppendLine(listItemModel);
                    }
                    templateContent = templateContent.Replace(completedIterationModel, replacedIteration.ToString());
                }
            }

            return templateContent;
        }

        private static string LoadResource(string embebedResource)
        {
            string templateContent;
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(embebedResource))
            using (StreamReader reader = new StreamReader(stream))
            {
                templateContent = reader.ReadToEnd();
            }
            return templateContent;
        }

        public Dictionary<string, string> AskForInputParameters(IConsoleService consoleService, Dictionary<string, string> paramsDescription)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            foreach (var item in paramsDescription)
            {
                consoleService.WriteLine($"{item.Value}:");
                var input = consoleService.ReadLine();
                output.Add(item.Key, input);
            }
            return output;
        }


        public List<Dictionary<string, string>> AskForIterationInputParameters(IConsoleService consoleService, string iterationDescription, Dictionary<string, string> paramsDescription)
        {
            List<Dictionary<string, string>> iterations = new List<Dictionary<string, string>>();
            bool moreInputs;
            consoleService.WriteLine($"\t (yes/no) {iterationDescription}. Would you like to add items?");
            var addItems = consoleService.ReadLine();
            if (!string.IsNullOrEmpty(addItems) && Definitions.AvailableTrueStrings.ToList().IndexOf(addItems.ToLowerInvariant()) == -1)
            {
                return iterations;
            }
            do
            {
                Dictionary<string, string> iteration = AskForInputParameters(consoleService, paramsDescription);
                iterations.Add(iteration);
                consoleService.WriteLine("(yes/no) Add more inputs?");
                string more = consoleService.ReadLine();
                moreInputs = !string.IsNullOrEmpty(more) && Definitions.AvailableTrueStrings.ToList().IndexOf(more.ToLowerInvariant()) > -1;
            } while (moreInputs);
            return iterations;
        }
    }
}
