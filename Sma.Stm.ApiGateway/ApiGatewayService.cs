using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sma.Stm.ApiGateway
{
    public class ApiGatewayService
    {
        public ApiGatewayService(IOptions<object> options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Options = options.Value;
            Client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false, UseCookies = false });
        }

        public object Options { get; private set; }

        internal HttpClient Client { get; private set; }

    }
}
