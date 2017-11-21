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
                            SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12,
                            ServerCertificate = new X509Certificate2("cert_digicert.pfx", "4Gr8Access!"),
                            CheckCertificateRevocation = true,
                            ClientCertificateValidation = Check,
                            ClientCertificateMode = ClientCertificateMode.RequireCertificate,
                        });
                    });
                    options.Listen(IPAddress.Any, 444, listenOptions =>
                    {
                        listenOptions.UseHttps(new HttpsConnectionAdapterOptions
                        {
                            SslProtocols = SslProtocols.Tls | SslProtocols.Tls11 | SslProtocols.Tls12,
                            ServerCertificate = new X509Certificate2("cert_mcp.pfx", "StmVis1234"),
                            CheckCertificateRevocation = true,
                            ClientCertificateValidation = Check,
                            ClientCertificateMode = ClientCertificateMode.RequireCertificate,
                        });
                    });
                })
                .Build();

        private static bool Check(X509Certificate2 cert, X509Chain chain, System.Net.Security.SslPolicyErrors policy)
        {
            chain.ChainPolicy = new X509ChainPolicy()
            {
                RevocationFlag = X509RevocationFlag.EndCertificateOnly,
                RevocationMode = X509RevocationMode.NoCheck,
                UrlRetrievalTimeout = new TimeSpan(0, 0, 10),
                VerificationTime = DateTime.UtcNow
            };

            var result = chain.Build(cert);
            if (result == true)
            {
                return true;
            }

            var errors = string.Empty;
            foreach (X509ChainStatus chainStatus in chain.ChainStatus)
            {
                errors += string.Format("Chain error: {0} {1}", chainStatus.Status, chainStatus.StatusInformation);
            }

            return true;
        }
    }
}