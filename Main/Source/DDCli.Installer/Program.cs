using DDCli.Installer.Models;
using DDCli.Installer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Installer
{
    class Program
    {
        private const string FilePath = "Assembly.zip";
        private const string ArtifactName = "Assembly";
        private const string ArtifactorExtractionFolder = "artifactor_extraction";
        private const string AssemblyCliExtractionFolder = "assembly_cli_extraction";
        private const string AssemblyCliDynamicsExtractionFolder = "assembly_cli_dynamics_extraction";
        private const string InstallCliFolder = @"C:\DDCli";
        private const string InstallCliDynamicsFolder = @"C:\DDCli.Dynamics";
        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();


            Console.WriteLine("Retrieving token from key vault...");
            var devOpsToken = GetKeyVaultDevOpsToken(configuration);

            var devOpsSettingSection = configuration.GetSection("DevOpsEnvironmentSettings");

            var organizationUri = GetSetting(devOpsSettingSection, "OrganizationUri");
            var projectName = GetSetting(devOpsSettingSection, "ProjecName");
            var projectGuid = GetSetting(devOpsSettingSection, "ProjectGuid");
            var username = GetSetting(devOpsSettingSection, "Username");

            Console.WriteLine("Connecting Dev Ops organization...");
            var devOpsService = new AzureDevOpsService(organizationUri, username, devOpsToken, new Guid(projectGuid));

            Console.WriteLine("Retrieving build data...");
            var lastBuild = devOpsService.GetProjectBuilds()
                .Result
                .OrderByDescending(k => k.LastChangedDate)
                .FirstOrDefault();

            Console.WriteLine("Downloading zip from artifactor...");

            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            CreateNewDirectory(ArtifactorExtractionFolder);
            //CreateNewDirectory(AssemblyCliExtractionFolder);

            Stream zipStream = devOpsService.GetArtifact(lastBuild.Id, ArtifactName).Result;
            using (FileStream zipFile = new FileStream(FilePath, FileMode.Append))
            {
                zipStream.CopyTo(zipFile);
            }
            

            Console.WriteLine("Unzipping artifactor...");
            ZipFile.ExtractToDirectory(FilePath, ArtifactorExtractionFolder);
            var assemblyCliZipFileName = $@"{ArtifactorExtractionFolder}\Assembly\DDCli\{lastBuild.Id}.zip";
            var assemblyCliDynamicsZipFileName = $@"{ArtifactorExtractionFolder}\Assembly\DDCli.Dynamics\{lastBuild.Id}.zip";

            Console.WriteLine("Unzipping assemblies...");

            Directory.Delete(AssemblyCliExtractionFolder, true);
            Directory.Delete(AssemblyCliDynamicsExtractionFolder, true);

            ZipFile.ExtractToDirectory(assemblyCliZipFileName, AssemblyCliExtractionFolder);
            ZipFile.ExtractToDirectory(assemblyCliDynamicsZipFileName, AssemblyCliDynamicsExtractionFolder);
            Console.WriteLine("Installing files in folder...");
            Directory.Delete(InstallCliFolder, true);
            Directory.Delete(InstallCliDynamicsFolder, true);
            Directory.Move(AssemblyCliExtractionFolder, InstallCliFolder);
            Directory.Move(AssemblyCliDynamicsExtractionFolder, InstallCliDynamicsFolder);
            Console.WriteLine("Installation complete");

        }

        private static void CreateNewDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);
        }


        public static string GetSetting(IConfigurationSection section, string key)
        {
            return section.GetSection(key).Value;
        }

        private static string GetKeyVaultDevOpsToken(IConfigurationRoot configuration)
        {
            var keyVaultSettings = configuration.GetSection("KeyVaultSettings");

            var keyVaultName = keyVaultSettings.GetSection("Name").Value;
            var keyVaultSecret = keyVaultSettings.GetSection("SecretName").Value;
            var keyVaultService = new KeyVaultService();

            var password = keyVaultService.GetValueSecretFromKeyVault(keyVaultName, keyVaultSecret);

            return password;
        }




        static private async Task ShowWorkItemDetails(VssConnection connection, int workItemId)
        {
            var witClient = connection.GetClient<BuildHttpClient>();
            try
            {
                var workitem = await witClient.GetBuildAsync(new Guid("7cc85a15-7f9c-4e84-b42e-68d1452c3afa"), 151);

               
            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                }
            }
        }



    }
}
