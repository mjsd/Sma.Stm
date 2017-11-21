using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Sma.Stm.Ssc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sma.Stm.Ssc.ApiGateway
{
    public class ApiGatewayMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ApiGatewayOptions _options;
        private readonly SeaSwimInstanceContextService _seaSwimInstanceContextService;

        public ApiGatewayMiddleware(RequestDelegate next, 
            IOptions<ApiGatewayOptions> options,
            SeaSwimInstanceContextService seaSwimInstanceContextService)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _seaSwimInstanceContextService = seaSwimInstanceContextService ?? throw new ArgumentNullException(nameof(seaSwimInstanceContextService));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Request.Method == "OPTIONS")
            {
                return;
            }

            var requestPath = context.Request.Path.Value.Replace("//", "/");

            ApiGatewayOptionsItem target = null;
            if (_options.Routes != null
                && _options.Routes.ContainsKey(requestPath))
            {
                target = _options.Routes[requestPath];
            }
            else
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("The requested resource was not found");
                return;
            }

            var scheme = target.Scheme;
            var host = target.Host;
            var path = target.Path;
            var queryString = target.QueryString;

            if (target.Scheme == null)
                scheme = context.Request.Scheme;
            if (target.Host == null)
                host = context.Request.Host.Value;
            if (target.Path == null)
                path = context.Request.Path.Value;
            if (target.QueryString == null)
                queryString = context.Request.QueryString.Value;

            var uri = new Uri(UriHelper.BuildAbsolute(scheme, new HostString(host), target.PathBase, path, new QueryString(queryString)));

            context.Request.Headers.Append("SeaSWIM-CallerOrgId", _seaSwimInstanceContextService.CallerOrgId);
            context.Request.Headers.Append("SeaSWIM-CallerServiceId", _seaSwimInstanceContextService.CallerServiceId);

            await context.ProxyRequest(uri);
        }
    }
}