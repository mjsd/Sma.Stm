using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Sma.Stm.Common.Web
{
    public static class WebRequestHelper
    {
        public static bool UseHmacAuthentication { get; set; }
        public static string ApiKey { get; set; }
        public static string AppId { get; set; }
        public static X509Certificate2 ClientCertificate { get; set; }

        public static bool IgnoreServerCertificateErrors { get; set; }

        public static WebResponse Get(string url, WebHeaderCollection headers = null, bool useCertificate = false)
        {
            var result = ExecuteWebRequest(url, null, "GET", headers, useCertificate);
            return result;
        }

        public static WebResponse Post(string url, string body, WebHeaderCollection headers = null, bool useCertificate = false)
        {
            var result = ExecuteWebRequest(url, body, "POST", headers, useCertificate);
            return result;
        }

        public static WebResponse Put(string url, string body, WebHeaderCollection headers = null, bool useCertificate = false)
        {
            var result = ExecuteWebRequest(url, body, "PUT", headers, useCertificate);
            return result;
        }

        public static WebResponse Delete(string url, string body, WebHeaderCollection headers = null, bool useCertificate = false)
        {
            var result = ExecuteWebRequest(url, body, "DELETE", headers, useCertificate);
            return result;
        }

        private static WebResponse ExecuteWebRequest(string url, string body, string method, WebHeaderCollection headers = null, bool useCertificate = false)
        {
            if (WebRequest.Create(url) is HttpWebRequest request)
            {
                request.Method = method;
                request.PreAuthenticate = true;
                request.Accept = "";

                if (headers != null)
                {
                    foreach (var key in headers.AllKeys)
                    {
                        if (key.ToLower() != "content-type")
                            continue;

                        request.ContentType = headers[key];
                        headers.Remove(key);
                    }

                    if (request.Headers == null)
                        request.Headers = headers;
                    else
                    {
                        foreach (var key in headers.AllKeys)
                            request.Headers.Add(key, headers[key]);
                    }
                }

                if (useCertificate)
                {
                    if (ClientCertificate == null)
                        throw new Exception("Client certificate has not been set");

                    request.ClientCertificates.Add(ClientCertificate);
                }

                if (UseHmacAuthentication)
                {
                    // Add HMAC authentication hader to the request
                    request = AddHamcAuthentication(request, body);
                }

                if (method == "POST" || method == "DELETE")
                {
                    var byteArray = Encoding.UTF8.GetBytes(body ?? string.Empty);
                    request.ContentLength = byteArray.Length;
                    using (var dataStream = request.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);
                    }
                }

                // Set server security validation callback
                ServicePointManager.ServerCertificateValidationCallback
                    = CheckServerCert;

                var result = new WebResponse();
                try
                {
                    var response = (HttpWebResponse) request.GetResponse();

                    result.HttpStatusCode = response.StatusCode;

                    using (var sr = new StreamReader(response.GetResponseStream()))
                    {
                        result.Body = sr.ReadToEnd();
                    }
                }
                catch (WebException wex)
                {
                    if (wex.Response != null)
                    {
                        using (var sr = new StreamReader(wex.Response.GetResponseStream()))
                        {
                            while (sr.Peek() > -1)
                            {
                                result.Body = sr.ReadToEnd();
                            }
                        }
                        result.HttpStatusCode = ((HttpWebResponse) wex.Response).StatusCode;
                    }
                    else
                    {
                        result.HttpStatusCode = HttpStatusCode.InternalServerError;
                    }

                    result.ErrorMessage = wex.Message;
                }

                return result;
            }

            return null;
        }

        private static HttpWebRequest AddHamcAuthentication(HttpWebRequest request, string body)
        {
            var requestContentBase64String = string.Empty;

            var requestUri = System.Web.HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            var requestHttpMethod = request.Method;

            // Calculate UNIX time
            var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            var timeSpan = DateTime.UtcNow - epochStart;
            var requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            var nonce = Guid.NewGuid().ToString("N");

            byte[] content = null;
            if (body != null)
                content = Encoding.UTF8.GetBytes(body);

            if (content != null)
            {
                var md5 = MD5.Create();
                //Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                var requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }

            // Creating the raw signature string
            var signatureRawData =
                $"{AppId}{requestHttpMethod}{requestUri}{requestTimeStamp}{nonce}{requestContentBase64String}";

            var secretKeyByteArray = Encoding.UTF8.GetBytes(ApiKey);

            var signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (var hmac = new HMACSHA256(secretKeyByteArray))
            {
                var signatureBytes = hmac.ComputeHash(signature);
                var requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                //Setting the values in the Authorization header using custom scheme (amx)
                request.Headers.Add("Authorization", string.Format("amx {0}:{1}:{2}:{3}", AppId, requestSignatureBase64String, nonce, requestTimeStamp));
            }

            return request;
        }

        public static string CombineUrl(string baseUrl, string stringToAdd)
        {
            baseUrl = baseUrl.TrimEnd('/');
            stringToAdd = stringToAdd.TrimStart('/');
            return string.Format("{0}/{1}", baseUrl, stringToAdd);
        }

        private static bool CheckServerCert(object sender, X509Certificate certification, X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            if (IgnoreServerCertificateErrors)
            {
                return true;
            }

            var result = chain.Build(new X509Certificate2(certification));
            if (result)
            {
                return true;
            }

            var errors = chain.ChainStatus.Aggregate(string.Empty, (current, chainStatus) => current + string.Format("Chain error: {0} {1}", chainStatus.Status, chainStatus.StatusInformation));
            return true;
        }

        public class WebResponse
        {
            public HttpStatusCode HttpStatusCode { get; set; }
            public string ErrorMessage { get; set; }
            public string Body { get; set; }
        }
    }
}