using Microsoft.Extensions.Configuration;
using Sma.Stm.Common.Security;
using Sma.Stm.Common.Web;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Sma.Stm.Ssc
{
    public class IdentityRegistryService
    {
        private readonly string idRegistryBaseUrl;
        private readonly string rootCertificateTumbprint;

        private const string IDREG_PATH_ORG_IDENTITIES = "/org/%s/services";


        public IdentityRegistryService(IConfiguration configuration)
        {
            idRegistryBaseUrl = configuration.GetValue<string>("IdREgistryBaseUrl");
        }

        public bool IsCertificateValid(X509Certificate2 cert)
        {
            var errors = string.Empty;
            return CertificateValidator.IsCertificateValid(cert, rootCertificateTumbprint, out errors);
        }

        public WebRequestHelper.WebResponse MakeGenericCall(string url, string method, string body = null, WebHeaderCollection headers = null)
        {
            WebRequestHelper.WebResponse response = null;

            url = idRegistryBaseUrl + url;
            if (method == "GET")
                response = WebRequestHelper.Get(url, headers, true);
            else if (method == "POST")
                response = WebRequestHelper.Post(url, body, headers: headers, UseCertificate: true);

            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new WebException(response.ErrorMessage);
            }

            return response;
        }
    }
}
