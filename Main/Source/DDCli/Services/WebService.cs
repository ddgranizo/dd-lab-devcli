using DDCli.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DDCli.Services
{
    public class WebService : IWebService
    {
        public WebService()
        {
        }

        public void DownloadFile(string url)
        {
            Uri uri = new Uri(url);
            if (uri.IsFile)
            {
                string filename = System.IO.Path.GetFileName(uri.LocalPath);
                using (var client = new WebClient())
                {
                    client.DownloadFile(url, filename);
                }
            }
            else
            {
                throw new ArgumentException("url should reference a file");
            }

        }
    }
}
