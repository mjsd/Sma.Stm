using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using RabbitMQ.Client;
using Swashbuckle.AspNetCore.Swagger;
using Sma.Stm.EventBus;
using Sma.Stm.EventBus.Abstractions;
using Sma.Stm.Services.AuthorizationService.DataAccess;
using Sma.Stm.EventBus.Events;
using Sma.Stm.EventBusRabbitMq;
using Sma.Stm.Services.NotificationService.IntegrationEvents.EventHandling;

namespace Sma.Stm.Services.NotificationService
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
            services.AddMvc();

            services.AddApiVersioning(option =>
            {
                option.ReportApiVersions = true;
                option.AssumeDefaultVersionWhenUnspecified = true;
                option.DefaultApiVersion = new ApiVersion(1, 0);
            });

            var sqlConnectionString = Configuration.GetConnectionString("DataAccessPostgreSqlProvider");
            services.AddDbContext<NotificationDbContext>(options =>
                    options.UseNpgsql(
                        sqlConnectionString,
                        b => b.MigrationsAssembly("Sma.Stm.Services.NotificationService")
                    )
                );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Notification API", Version = "v1" });
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

            var db = sp.GetRequiredService<NotificationDbContext>();
            db.Database.MigrateAsync();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API V1");
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
            services.AddTransient<NotificationIntegrationEventHandler>();
        }

        private void ConfigureEventBus(IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<NotificationIntegrationEvent, NotificationIntegrationEventHandler>();
        }
    }
}