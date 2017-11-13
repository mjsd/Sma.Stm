﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Options;
using Sma.Stm.Ssc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Sma.Stm.ApiGateway
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

            var requestPath = context.Request.Path;

            ApiGatewayOptionsItem target = null;
            if (_options.Routes != null
                && _options.Routes.ContainsKey(requestPath))
            {
                target = _options.Routes[requestPath];
            }
            else
            {
                await Task.FromResult(0);
                return;
            }

            var scheme = target.Scheme;
            var host = target.Host;
            var path = target.Path;
            var queryString = target.QueryString;

            if (string.IsNullOrEmpty(target.Scheme))
                scheme = context.Request.Scheme;
            if (string.IsNullOrEmpty(target.Host))
                host = context.Request.Host.Value;
            if (string.IsNullOrEmpty(target.Path))
                path = context.Request.Path.Value;
            if (string.IsNullOrEmpty(target.QueryString))
                queryString = context.Request.QueryString.Value;

            var uri = new Uri(UriHelper.BuildAbsolute(scheme, new HostString(host), target.PathBase, path, new QueryString(queryString)));

            context.Request.Headers.Append("SeaSWIM-CallerOrgId", _seaSwimInstanceContextService.CallerOrgId);
            context.Request.Headers.Append("SeaSWIM-CallerServiceId", _seaSwimInstanceContextService.CallerServiceId);

            await context.ProxyRequest(uri);
        }
    }
}