using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Installer.Services
{
    public class AzureDevOpsService
    {
        public string OrganizationUrl { get; }
        public string Username { get; }
        public string Token { get; }
        public Guid ProjectId { get; }

        public VssConnection Connection { get; set; }

        public AzureDevOpsService(string organizationUrl, string username, string token, Guid projectId)
        {
            VssClientCredentials clientCredentials =
                new VssClientCredentials(new VssBasicCredential(username, token));
            OrganizationUrl = organizationUrl ?? throw new ArgumentNullException(nameof(organizationUrl));
            Username = username ?? throw new ArgumentNullException(nameof(username));
            Token = token ?? throw new ArgumentNullException(nameof(token));
            ProjectId = projectId;
            Connection = new VssConnection(new Uri(OrganizationUrl), clientCredentials);
        }



        public async Task<Stream> GetArtifact(int buildId, string artifactName)
        {
            var witClient = Connection.GetClient<BuildHttpClient>( );
            try
            {
                return await witClient.GetArtifactContentZipAsync(ProjectId.ToString(), buildId, artifactName);
            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                }
                throw;
            }
        }

        public async Task<IPagedList<Build>> GetProjectBuilds()
        {
            var witClient = Connection.GetClient<BuildHttpClient>();
            try
            {
                return await witClient.GetBuildsAsync2(ProjectId.ToString());
            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                }
                throw;
            }
        }


        public async Task<TeamProject> GetCurrentProject()
        {
            var witClient = Connection.GetClient<ProjectHttpClient>();
            try
            {
                return await witClient.GetProject(ProjectId.ToString());
            }
            catch (AggregateException aex)
            {
                VssServiceException vssex = aex.InnerException as VssServiceException;
                if (vssex != null)
                {
                    Console.WriteLine(vssex.Message);
                }
                throw;
            }
        }


    }
}
