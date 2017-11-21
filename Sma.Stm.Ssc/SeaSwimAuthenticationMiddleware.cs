using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sma.Stm.Common.Security;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Sma.Stm.Ssc
{
    public class SeaSwimAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly object _options;

        public SeaSwimAuthenticationMiddleware(RequestDelegate next, IOptions<object> options)
        {
            if (next == null)
            {
                throw new ArgumentNullException(nameof(next));
            }
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Connection.ClientCertificate == null)
            {
                // throw new ArgumentNullException("Client certificate");
                var instanceContextService = context.RequestServices.GetRequiredService<SeaSwimInstanceContextService>();
                instanceContextService.CallerOrgId = "orgid";
                instanceContextService.CallerServiceId = "serviceid";

                await _next.Invoke(context);
                return;
            }

            var errors = string.Empty;
            if (CertificateValidator.IsCertificateValid(context.Connection.ClientCertificate, "ac1f8f49454e46a00d0f1e12fc4f689bf6726b0a", out errors))
            {
                var instanceContextService = context.RequestServices.GetRequiredService<SeaSwimInstanceContextService>();
                instanceContextService.CallerOrgId = GetOrgId(context.Connection.ClientCertificate);
                instanceContextService.CallerServiceId = GetServiceId(context.Connection.ClientCertificate);

                await _next.Invoke(context);
            }
            else
            {
                context.Response.Clear();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync($"Unauthorized {errors}");
            }
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