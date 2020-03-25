using DDCli.Interfaces;
using DDCli.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DDCli.Commands.Dev.DotNet
{
    public class AddCRUDDomainServicesCommand : CommandBase
    {
        private const string HelpDefinition = "Create basic structure CRUD domains services";
        public const string CreateService = "DDCli.Resources.DotNet.Domain.Implementations.CreateService.txt";
        public const string DeleteService = "DDCli.Resources.DotNet.Domain.Implementations.DeleteService.txt";
        public const string UpdateService = "DDCli.Resources.DotNet.Domain.Implementations.UpdateService.txt";
        public const string GetService = "DDCli.Resources.DotNet.Domain.Implementations.GetService.txt";
        public const string GetMultipleService = "DDCli.Resources.DotNet.Domain.Implementations.GetMultipleService.txt";

        public const string CreateInterface = "DDCli.Resources.DotNet.Domain.Interfaces.CreateInterfaceService.txt";
        public const string DeleteInterface = "DDCli.Resources.DotNet.Domain.Interfaces.DeleteInterfaceService.txt";
        public const string UpdateInterface = "DDCli.Resources.DotNet.Domain.Interfaces.UpdateInterfaceService.txt";
        public const string GetInterface = "DDCli.Resources.DotNet.Domain.Interfaces.GetInterfaceService.txt";
        public const string GetMultipleInterface = "DDCli.Resources.DotNet.Domain.Interfaces.GetMultipleInterfaceService.txt";

        public AddCRUDDomainServicesCommand(IFileService fileService, ITemplateReplacementService templateReplacementService)
            : base(typeof(AddCRUDDomainServicesCommand).Namespace, nameof(AddCRUDDomainServicesCommand), HelpDefinition)
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
                    { "ImplementationsNamespace", "Namespace for implementations services"},
                    { "InterfacesNamespace" , "Namespace for interfaces services"},
                    { "EntityName", "Entity name"}
                });

            var className = replacements["EntityName"];

            var contentCreateInterface = TemplateReplacementService.Replace(CreateInterface, replacements);
            var contentUpdateInterface = TemplateReplacementService.Replace(UpdateInterface, replacements);
            var contentDeleteInterface = TemplateReplacementService.Replace(DeleteInterface, replacements);
            var contentGetInterface = TemplateReplacementService.Replace(GetInterface, replacements);
            var contentGetMultipleInterface = TemplateReplacementService.Replace(GetMultipleInterface, replacements);

            var contentCreateService = TemplateReplacementService.Replace(CreateService, replacements);
            var contentUpdateService = TemplateReplacementService.Replace(UpdateService, replacements);
            var contentDeleteService = TemplateReplacementService.Replace(DeleteService, replacements);
            var contentGetService = TemplateReplacementService.Replace(GetService, replacements);
            var contentGetMultipleService = TemplateReplacementService.Replace(GetMultipleService, replacements);

            var pathCreateInterface = FileService.GetAbsoluteCurrentPath($"ICreate{className}Service.cs");
            var pathUpdateInterface = FileService.GetAbsoluteCurrentPath($"IUpdate{className}Service.cs");
            var pathDeleteInterface = FileService.GetAbsoluteCurrentPath($"IDelete{className}Service.cs");
            var pathGetInterface = FileService.GetAbsoluteCurrentPath($"IGet{className}Service.cs");
            var pathGetMultipleInterface = FileService.GetAbsoluteCurrentPath($"IGetMultiple{className}Service.cs");

            var pathCreateService = FileService.GetAbsoluteCurrentPath($"Create{className}Service.cs");
            var pathUpdateService = FileService.GetAbsoluteCurrentPath($"Update{className}Service.cs");
            var pathDeleteService = FileService.GetAbsoluteCurrentPath($"Delete{className}Service.cs");
            var pathGetService = FileService.GetAbsoluteCurrentPath($"Get{className}Service.cs");
            var pathGetMultipleService = FileService.GetAbsoluteCurrentPath($"GetMultiple{className}Service.cs");

            var createdCreateInterface = FileService.WriteFile(pathCreateInterface, contentCreateInterface, false);
            var createdUpdateInterface = FileService.WriteFile(pathUpdateInterface, contentUpdateInterface, false);
            var createdDeleteInterface = FileService.WriteFile(pathDeleteInterface, contentDeleteInterface, false);
            var createdGetInterface = FileService.WriteFile(pathGetInterface, contentGetInterface, false);
            var createdGetMultipleInterface = FileService.WriteFile(pathGetMultipleInterface, contentGetMultipleInterface, false);

            var createdCreateService = FileService.WriteFile(pathCreateService, contentCreateService, false);
            var createdUpdateService = FileService.WriteFile(pathUpdateService, contentUpdateService, false);
            var createdDeleteService = FileService.WriteFile(pathDeleteService, contentDeleteService, false);
            var createdGetService = FileService.WriteFile(pathGetService, contentGetService, false);
            var createdGetMultipleService = FileService.WriteFile(pathGetMultipleService, contentGetMultipleService, false);


            var filesCreated = string.Join("\r\n", new string[] {
                createdCreateInterface,
                createdUpdateInterface,
                createdDeleteInterface,
                createdGetInterface,
                createdGetMultipleInterface,
                createdCreateService,
                createdUpdateService,
                createdDeleteService,
                createdGetService,
                createdGetMultipleService
            });
            ConsoleService.WriteLine($"Created files:\r\n{filesCreated}");
        }
    }
}
