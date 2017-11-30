using System;
using Microsoft.Extensions.Configuration;
using Sma.Stm.Common.Security;
using Sma.Stm.Common.Web;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace Sma.Stm.Ssc
{
    public class IdentityRegistryService
    {
        private readonly string _idRegistryBaseUrl;
        private readonly string _rootCertificateTumbprint;

        private const string IdregPathOrgIdentities = "/org/%s/services";

        public IdentityRegistryService(IConfiguration configuration)
        {
            _idRegistryBaseUrl = configuration.GetValue<string>("IdREgistryBaseUrl");
        }

        public WebRequestHelper.WebResponse MakeGenericCall(string url, string method, string body = null, WebHeaderCollection headers = null)
        {
            WebRequestHelper.WebResponse response = null;

            url = _idRegistryBaseUrl + url;
            switch (method)
            {
                case "GET":
                    response = WebRequestHelper.Get(url, headers, true);
                    break;
                case "POST":
                    response = WebRequestHelper.Post(url, body, headers: headers, useCertificate: true);
                    break;
                default:
                    throw new Exception($"Http method {method} is not alowed");
            }

            if (response != null && response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new WebException(response.ErrorMessage);
            }

            return response;
        }
    }
}
