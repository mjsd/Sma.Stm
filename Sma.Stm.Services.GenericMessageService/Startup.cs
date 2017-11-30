using Autofac.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using RabbitMQ.Client;
using Sma.Stm.EventBus.Abstractions;
using Autofac;
using Sma.Stm.EventBus;
using Sma.Stm.EventBus.Events;
using Sma.Stm.Services.GenericMessageService.IntegrationEvents.EventHandling;
using Microsoft.AspNetCore.Mvc;
using Sma.Stm.Ssc;
using Sma.Stm.Services.GenericMessageService.DataAccess;
using Microsoft.EntityFrameworkCore;
using Sma.Stm.Common.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Sma.Stm.EventBusRabbitMq;

namespace Sma.Stm.Services.GenericMessageService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new TextMediaTypeFormatter());
            });

            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            });


            services.AddScoped<SeaSwimInstanceContextService>();
            services.AddSingleton<SeaSwimIdentityService>();

            var sqlConnectionString = Configuration.GetConnectionString("DataAccessPostgreSqlProvider");
            services.AddDbContext<GenericMessageDbContext>(options =>
                    options.UseNpgsql(
                        sqlConnectionString,
                        b => b.MigrationsAssembly("Sma.Stm.Services.GenericMessageService")
                    )
                );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Generic Message Service API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
                c.OperationFilter<Common.Swagger.RequestContentTypeOperationFilter>();
                c.OperationFilter<Common.Swagger.ResponseContentTypeOperationFilter>();
            });

            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = Configuration["EventBusConnection"]
                };

                if (!string.IsNullOrEmpty(Configuration["EventBusUserName"]))
                {
                    factory.UserName = Configuration["EventBusUserName"];
                }

                if (!string.IsNullOrEmpty(Configuration["EventBusPassword"]))
                {
                    factory.Password = Configuration["EventBusPassword"];
                }

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new DefaultRabbitMqPersistentConnection(factory, logger, retryCount);
            });

            RegisterEventBus(services);

            var container = new ContainerBuilder();
            container.Populate(services);
            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env, IServiceProvider sp)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var db = sp.GetRequiredService<GenericMessageDbContext>();
            db.Database.MigrateAsync();

            app.UseSeaSwimAuthentication(new object());

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Generic Message Service API V1");
            });

            app.UseMvc();
            ConfigureEventBus(app);
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            services.AddSingleton<IEventBus, EventBusRabbitMq.EventBusRabbitMq>(sp =>
            {
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq.EventBusRabbitMq>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (!string.IsNullOrEmpty(Configuration["EventBusRetryCount"]))
                {
                    retryCount = int.Parse(Configuration["EventBusRetryCount"]);
                }

                return new EventBusRabbitMq.EventBusRabbitMq(rabbitMqPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<NewSubscriptionIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NewSubscriptionIntegrationEvent, NewSubscriptionIntegrationEventHandler>();
        }
    }
}