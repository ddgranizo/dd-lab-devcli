using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Utilities
{
    public static class CrmProvider
    {

        public static void MigrateEntities(Action<string> loggerHandler, IOrganizationService from, IOrganizationService to, string[] entities)
        {
            MigrationProvider.Migrate(loggerHandler, from, to, entities);
        }

        public static void CloneUsdConfiguration(Action<string> loggerHandler, IOrganizationService from, IOrganizationService to, bool includeOptions)
        {
            UsdConfigurationProvider.CloneUsdConfiguration(loggerHandler, from, to, includeOptions);
        }

        public static string GetServiceDisplayName(string stringConnection)
        {
            CrmServiceClient crmService = new CrmServiceClient(stringConnection);
            var uri = crmService.CrmConnectOrgUriActual;
            return $"{uri.Host}/{crmService.ConnectedOrgUniqueName}";
        }

        public static IOrganizationService GetService(string stringConnection)
        {
            CrmServiceClient crmService = new CrmServiceClient(stringConnection);
            IOrganizationService serviceProxy = crmService.OrganizationWebProxyClient != null ?
                                                        crmService.OrganizationWebProxyClient :
                                                        (IOrganizationService)crmService.OrganizationServiceProxy;
            return serviceProxy ??
                 throw new Exception("Can't initialize Service with provided string connection"); ;
        }


        public static CrmServiceClient GetCrmServiceClient(string stringConnection)
        {
            CrmServiceClient crmService = new CrmServiceClient(stringConnection);
            return crmService;
        }



        public static string GetCrmToken(string username, string password, string url, string clientId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new
                    Uri("https://login.microsoftonline.com/common/oauth2/token");

                HttpRequestMessage request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;

                var keysValues = new List<KeyValuePair<string, string>>();
                keysValues.Add(new KeyValuePair<string, string>("client_id", clientId));
                keysValues.Add(new KeyValuePair<string, string>("resource", url));
                keysValues.Add(new KeyValuePair<string, string>("username", username));
                keysValues.Add(new KeyValuePair<string, string>("password", password));
                keysValues.Add(new KeyValuePair<string, string>("grant_type", "password"));

                request.Content = new FormUrlEncodedContent(keysValues);

                HttpResponseMessage response = client.SendAsync(request).Result;
                //TODO: complete the request
                return "";
            }
        }


        public static void ImportSolutions(IOrganizationService service, byte[] data, bool overWriteUnmanagedCustomizations, bool migrateAsHold, bool publishWorkflows)
        {
            ImportSolutionRequest importRequest = new ImportSolutionRequest()
            {
                CustomizationFile = data,
                OverwriteUnmanagedCustomizations = overWriteUnmanagedCustomizations,
                HoldingSolution = migrateAsHold,
                PublishWorkflows = publishWorkflows,
            };
            service.Execute(importRequest);
        }

        public static void ImportSolutionsAsync(IOrganizationService service, byte[] data, bool overWriteUnmanagedCustomizations, bool migrateAsHold, bool publishWorkflows)
        {
            var jobId = ImportSolutionAsyncRequest(
                        service,
                        data,
                        overWriteUnmanagedCustomizations,
                        migrateAsHold,
                        publishWorkflows);
            WaitAsnycOperation(service, jobId);
        }

        private static Guid ImportSolutionAsyncRequest(
                IOrganizationService service,
                byte[] data,
                bool overwriteUnmanagedCustomizations = true,
                bool migrateAsHold = false,
                bool publishWorkflows = true)
        {
            ImportSolutionRequest importRequest = new ImportSolutionRequest()
            {
                CustomizationFile = data,
                OverwriteUnmanagedCustomizations = overwriteUnmanagedCustomizations,
                HoldingSolution = migrateAsHold,
                PublishWorkflows = publishWorkflows,
            };

            ExecuteAsyncRequest asyncRequest = new ExecuteAsyncRequest()
            {
                Request = importRequest,

            };
            var asyncResponse = (ExecuteAsyncResponse)service.Execute(asyncRequest);
            var asyncJobId = asyncResponse.AsyncJobId;
            return asyncJobId;
        }

        private static void WaitAsnycOperation(IOrganizationService service, Guid jobId, int maxTimeOut = 1000 * 60 * 200)
        {
            DateTime end = DateTime.Now.AddMilliseconds(maxTimeOut);
            bool completed = false;
            while (!completed && end >= DateTime.Now)
            {
                System.Threading.Thread.Sleep(200);
                try
                {
                    Entity asyncOperation = service.Retrieve("asyncoperation", jobId, new ColumnSet(true));
                    var statusCode = asyncOperation.GetAttributeValue<OptionSetValue>("statuscode").Value;
                    if (statusCode == 30)
                    {
                        completed = true;
                    }
                    else if (statusCode == 21
                            || statusCode == 22
                            || statusCode == 31
                            || statusCode == 32)
                    {
                        throw new Exception(
                                string.Format(
                                    "Async oepration failed: {0} {1}",
                                    statusCode,
                                    asyncOperation.GetAttributeValue<string>("message")));
                    }
                }
                catch (TimeoutException)
                {
                    //do nothign
                }
                catch (FaultException)
                {
                    //Do nothing
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
        }





    }
}
