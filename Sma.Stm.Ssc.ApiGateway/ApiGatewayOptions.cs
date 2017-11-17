using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.Ssc.ApiGateway
{
    public class ApiGatewayOptions
    {
        public IDictionary<string, ApiGatewayOptionsItem> Routes { get; set; }
    }

    public class ApiGatewayOptionsItem
    {
        /// <summary>
        /// Destination uri scheme
        /// </summary>
        public string Scheme { get; set; }

        /// <summary>
        /// Destination uri host
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Destination uri path base to which current Path will be appended
        /// </summary>
        public string PathBase { get; set; }

        public string Path { get; set; }

        /// <summary>
        /// Query string parameters to append to each request
        /// </summary>
        public string QueryString { get; set; }
    }
}