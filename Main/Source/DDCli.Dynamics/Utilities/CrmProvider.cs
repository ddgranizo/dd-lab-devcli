using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DDCli.Dynamics.Utilities
{
    public static class CrmProvider
    {
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


    }
}
