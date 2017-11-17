using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;
using System.Security.Authentication;

namespace Sma.Stm.Ssc.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseIISIntegration()
                .UseStartup<Startup>()
                .ConfigureLogging((hostingContext, builder) =>
                {
                    builder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    builder.AddConsole();
                    builder.AddDebug();
                })
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, 443, listenOptions =>
                    {
                        listenOptions.UseHttps(new HttpsConnectionAdapterOptions
                        {
                            ServerCertificate = new X509Certificate2("cert.pfx", "4Gr8Access!"),
                            //CheckCertificateRevocation = true,
                            //ClientCertificateMode = ClientCertificateMode.RequireCertificate,
                        });
                    });
                })
                .Build();
    }
}