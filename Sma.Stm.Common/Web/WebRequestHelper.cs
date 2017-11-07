using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Sma.Stm.Common.Web
{
    public static class WebRequestHelper
    {
        public static bool UseHMACAuthentication { get; set; }
        public static string APIKey { get; set; }
        public static string APPId { get; set; }
        private static X509Certificate2 ClientCertificate { get; set; }

        public static bool IgnoreServerCertificateErrors { get; set; }

        public static WebResponse Get(string url, WebHeaderCollection headers = null, bool UseCertificate = false)
        {
            var result = ExecuteWebRequest(url, null, "GET", headers, UseCertificate);
            return result;
        }

        public static WebResponse Post(string url, string body, WebHeaderCollection headers = null, bool UseCertificate = false)
        {
            var result = ExecuteWebRequest(url, body, "POST", headers, UseCertificate);
            return result;
        }

        public static WebResponse Put(string url, string body, WebHeaderCollection headers = null, bool UseCertificate = false)
        {
            var result = ExecuteWebRequest(url, body, "PUT", headers, UseCertificate);
            return result;
        }

        public static WebResponse Delete(string url, string body, WebHeaderCollection headers = null, bool UseCertificate = false)
        {
            var result = ExecuteWebRequest(url, body, "DELETE", headers, UseCertificate);
            return result;
        }

        private static WebResponse ExecuteWebRequest(string url, string body, string method, WebHeaderCollection headers = null, bool UseCertificate = false)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = method;
            request.PreAuthenticate = true;
            request.Accept = "";

            if (headers != null)
            {
                foreach (string key in headers.AllKeys)
                {
                    if (key.ToLower() == "content-type")
                    {
                        request.ContentType = headers[key];
                        headers.Remove(key);
                    }
                }

                if (request.Headers == null)
                    request.Headers = headers;
                else
                {
                    foreach (string key in headers.AllKeys)
                        request.Headers.Add(key, headers[key]);
                }
            }

            if (UseCertificate)
            {
                if (ClientCertificate == null)
                    throw new Exception("Client certificate has not been set");

                request.ClientCertificates.Add(ClientCertificate);
            }

            if (UseHMACAuthentication)
            {
                // Add HMAC authentication hader to the request
                request = AddHAMCAuthentication(request, body);
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
                = new System.Net.Security.RemoteCertificateValidationCallback(CheckServerCert);

            var result = new WebResponse();
            try
            {
                var response = (HttpWebResponse)request.GetResponse();

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
                    result.HttpStatusCode = ((HttpWebResponse)wex.Response).StatusCode;
                }
                else
                {
                    result.HttpStatusCode = HttpStatusCode.InternalServerError;
                }

                result.ErrorMessage = wex.Message;
            }

            return result;
        }

        private static HttpWebRequest AddHAMCAuthentication(HttpWebRequest request, string body)
        {
            string requestContentBase64String = string.Empty;

            string requestUri = System.Web.HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

            string requestHttpMethod = request.Method;

            // Calculate UNIX time
            DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan timeSpan = DateTime.UtcNow - epochStart;
            string requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            //create random nonce for each request
            string nonce = Guid.NewGuid().ToString("N");

            byte[] content = null;
            if (body != null)
                content = Encoding.UTF8.GetBytes(body);

            if (content != null)
            {
                MD5 md5 = MD5.Create();
                //Hashing the request body, any change in request body will result in different hash, we'll incure message integrity
                byte[] requestContentHash = md5.ComputeHash(content);
                requestContentBase64String = Convert.ToBase64String(requestContentHash);
            }

            // Creating the raw signature string
            string signatureRawData = string.Format("{0}{1}{2}{3}{4}{5}", APPId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

            var secretKeyByteArray = Encoding.UTF8.GetBytes(APIKey);

            byte[] signature = Encoding.UTF8.GetBytes(signatureRawData);

            using (HMACSHA256 hmac = new HMACSHA256(secretKeyByteArray))
            {
                byte[] signatureBytes = hmac.ComputeHash(signature);
                string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);
                //Setting the values in the Authorization header using custom scheme (amx)
                request.Headers.Add("Authorization", string.Format("amx {0}:{1}:{2}:{3}", APPId, requestSignatureBase64String, nonce, requestTimeStamp));
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
            if (result == true)
            {
                return true;
            }

            var errors = string.Empty;
            foreach (X509ChainStatus chainStatus in chain.ChainStatus)
            {
                errors += string.Format("Chain error: {0} {1}", chainStatus.Status, chainStatus.StatusInformation);
            }

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
