using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using Sma.Stm.Ssc;

namespace Sma.Stm.ApiGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. 
        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiGateway();
            services.AddASeaSwimInstanceConfiguration();
        }

        // This method gets called by the runtime. 
        //Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            var json = File.ReadAllText(Path.Combine(env.ContentRootPath, @"routeconfiguration.json"));
            var options = JsonConvert.DeserializeObject<ApiGatewayOptions>(json);

            app.UseSeaSwimAuthentication(new object());
            app.RunApiGateway(options);
        }
    }
}