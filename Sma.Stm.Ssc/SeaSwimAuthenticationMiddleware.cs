using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Net;

namespace Sma.Stm.Ssc
{
    public class SeaSwimAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly object _options;

        public SeaSwimAuthenticationMiddleware(RequestDelegate next, IOptions<object> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var instanceContextService = context.RequestServices.GetRequiredService<SeaSwimInstanceContextService>();

            var header = context.Request.Headers["X-SSL-CERT"];

            if (string.IsNullOrEmpty(header))
            { 
                instanceContextService.IsAuthenticated = false;
                await _next.Invoke(context);
                return;
            }

            var decoded = WebUtility.UrlDecode(header).Replace("{", "").Replace("}", "");
            var cert = new X509Certificate2(Encoding.UTF8.GetBytes(decoded));

            instanceContextService.IsAuthenticated = false;
            instanceContextService.CallerOrgId = GetOrgId(cert);
            instanceContextService.CallerServiceId = GetServiceId(cert);

            await _next.Invoke(context);
        }

        private string GetOrgId(X509Certificate2 cert)
        {
            var certData = cert.Subject.Split(',');
            foreach (var item in certData)
            {
                var parts = item.Split('=');
                if (parts != null && parts.Count() == 2)
                {
                    if (parts[0].Trim() == "O")
                    {
                        return parts[1];
                    }
                }
            }

            throw new Exception("Org id not found in certificate");
        }

        private string GetServiceId(X509Certificate2 cert)
        {
            var certData = cert.Subject.Split(',');
            foreach (var item in certData)
            {
                var parts = item.Split('=');
                if (parts != null && parts.Count() == 2)
                {
                    if(parts[0].Trim().StartsWith("userId"))
                    {
                        return parts[1];
                    }
                }
            }

            throw new Exception("Service id not found in certificate");
        }
    }
}