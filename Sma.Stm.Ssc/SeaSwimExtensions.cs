using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;
using System;

namespace Sma.Stm.Ssc
{
    public static class SeaSwimExtensions
    {
        public static void UseSeaSwimAuthentication(this IApplicationBuilder app, object options)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            app.UseMiddleware<SeaSwimAuthenticationMiddleware>(Options.Create(options));
        }

    }
}
