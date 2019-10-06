using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.Dev.DotNet
{
    public class AddWPFUserControlCommand : CommandBase
    {
        private const string HelpDefinition = "Create basic structure for WPF User control";
        public const string ViewTemplate = "DDCli.Resources.DotNet.WpfUserControl.WpfUserControlViewTemplate.txt";
        public const string ViewModelTemplate = "DDCli.Resources.DotNet.WpfUserControl.WpfUserControlViewModelTemplate.txt";
        public const string ControllerTemplate = "DDCli.Resources.DotNet.WpfUserControl.WpfUserControlControllerTemplate.txt";

        public AddWPFUserControlCommand(IFileService fileService, ITemplateReplacementService templateReplacementService)
            : base(typeof(AddWPFUserControlCommand).Namespace, nameof(AddWPFUserControlCommand), HelpDefinition)
        {
            FileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
            TemplateReplacementService = templateReplacementService ?? throw new ArgumentNullException(nameof(templateReplacementService));
        }

        public IFileService FileService { get; }
        public ITemplateReplacementService TemplateReplacementService { get; }

        public override bool CanExecute(List<CommandParameter> parameters)
        {
            return true;
        }

        public override void Execute(List<CommandParameter> parameters)
        {
            var replacements = TemplateReplacementService
                .AskForInputParameters(ConsoleService, new Dictionary<string, string>()
                {
                    { "ViewNameSpace", "Write the 'View' class namespace"},
                    { "ViewModelNameSpace" , "Write the 'ViewModel' class namespace"},
                    { "ClassName", "Write the class name"}
                });

            var iterationProperties = TemplateReplacementService
                .AskForIterationInputParameters(ConsoleService, "Iteration for user control 'Props'",  new Dictionary<string, string>()
                {
                    { "PropertyType", "Input parameter type" },
                    { "PropertyName", "Input parameter name" },
                });

            var iterationEvents = TemplateReplacementService
               .AskForIterationInputParameters(ConsoleService, "Iteration for user control 'Events'", new Dictionary<string, string>()
               {
                    { "EventArgsType", "Event args type" },
                    { "EventName", "Event name" },
               });

            var iterations = new Dictionary<string, List<Dictionary<string, string>>>();
            iterations.Add("i", iterationProperties);
            iterations.Add("j", iterationEvents);

            var className = replacements["ClassName"];

            var contentController = TemplateReplacementService.Replace(ControllerTemplate, replacements, iterations);
            var contentViewModel = TemplateReplacementService.Replace(ViewModelTemplate, replacements, iterations);
            var contentView = TemplateReplacementService.Replace(ViewTemplate, replacements, iterations);

            var pathViewModel = FileService.GetAbsoluteCurrentPath($"{className}ViewModel.cs");
            var pathController = FileService.GetAbsoluteCurrentPath($"{className}View.xaml.cs");
            var pathView = FileService.GetAbsoluteCurrentPath($"{className}View.xaml");

            var createdControllerFile = FileService.WriteFile(pathController, contentController, false);
            var createdViewModelFile = FileService.WriteFile(pathViewModel, contentViewModel,false);
            var createdViewFile = FileService.WriteFile(pathView, contentView, false);

            var filesCreated = string.Join("\r\n", new string[] { createdViewFile, createdControllerFile, createdViewModelFile });
            ConsoleService.WriteLine($"Created files:\r\n{filesCreated}");
        }
    }
}
