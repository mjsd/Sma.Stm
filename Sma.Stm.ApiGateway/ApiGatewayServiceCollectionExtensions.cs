using Microsoft.Extensions.DependencyInjection;
using Sma.Stm.Ssc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sma.Stm.ApiGateway
{
    public static class ApiGatewayServiceCollectionExtensions
    {
        public static IServiceCollection AddApiGateway(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddSingleton<ApiGatewayService>();
        }

        public static IServiceCollection AddASeaSwimInstanceConfiguration(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddSingleton<SeaSwimInstanceContextService>();
        }
    }
}