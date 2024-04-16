using AutoMapper;
using DFC.Common.SharedContent.Pkg.Netcore;
using DFC.Common.SharedContent.Pkg.Netcore.Infrastructure;
using DFC.Common.SharedContent.Pkg.Netcore.Infrastructure.Strategy;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.Common.SharedContent.Pkg.Netcore.RequestHandler;
using DFC.Compui.Telemetry;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Content.Pkg.Netcore.Extensions;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;

namespace DFC.App.Triagetool
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string RedisCacheConnectionStringAppSettings = "Cms:RedisCacheConnectionString";
        private const string GraphApiUrlAppSettings = "Cms:GraphApiUrl";
        private const string WorkerThreadsConfigAppSettings = "ThreadSettings:WorkerThreads";
        private const string IocpThreadsConfigAppSettings = "ThreadSettings:IocpThreads";
        private readonly ILogger<Startup> logger;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            this.configuration = configuration;
            this.env = env;
            this.logger = logger;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();

                // add the default route
                endpoints.MapControllerRoute("default", "{controller=Health}/{action=Ping}");
            });
            mapper?.ConfigurationProvider.AssertConfigurationIsValid();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureMinimumThreads();
            services.AddStackExchangeRedisCache(options => { options.Configuration = configuration.GetSection(RedisCacheConnectionStringAppSettings).Get<string>(); });

            services.AddHttpClient();
            var redisCacheConnectionString = ConfigurationOptions.Parse(configuration.GetSection(RedisCacheConnectionStringAppSettings).Get<string>() ??
               throw new ArgumentNullException($"{nameof(RedisCacheConnectionStringAppSettings)} is missing or has an invalid value."));

            services.AddStackExchangeRedisCache(options => { options.Configuration = configuration.GetSection(RedisCacheConnectionStringAppSettings).Get<string>(); });
            services.AddSingleton<IConnectionMultiplexer>(option =>
            ConnectionMultiplexer.Connect(new ConfigurationOptions
            {
                EndPoints = { redisCacheConnectionString.EndPoints[0] },
                AbortOnConnectFail = false,
                Ssl = true,
                Password = redisCacheConnectionString.Password,
            }));
            services.AddHealthChecks().AddCheck<HealthCheck>("GraphQlRedisConnectionCheck");

            services.AddSingleton<IGraphQLClient>(s =>
            {
                var option = new GraphQLHttpClientOptions()
                {
                    EndPoint = new Uri(configuration.GetSection(GraphApiUrlAppSettings).Get<string>()),
                    HttpMessageHandler = new CmsRequestHandler(s.GetService<IHttpClientFactory>(), s.GetService<IConfiguration>(), s.GetService<IHttpContextAccessor>()),
                };
                var client = new GraphQLHttpClient(option, new NewtonsoftJsonSerializer());
                return client;
            });

            services.AddSingleton<ISharedContentRedisInterfaceStrategy<SharedHtml>, SharedHtmlQueryStrategy>();
            services.AddSingleton<ISharedContentRedisInterfaceStrategy<TriageToolFilterResponse>, TriageToolAllQueryStrategy>();
            services.AddSingleton<ISharedContentRedisInterfaceStrategy<TriagePageResponse>, PagesByTriageToolFilterStrategy>();
            services.AddSingleton<ISharedContentRedisInterfaceStrategyFactory, SharedContentRedisStrategyFactory>();
            services.AddScoped<ISharedContentRedisInterface, SharedContentRedis>();

            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSingleton(configuration.GetSection(nameof(CmsApiClientOptions)).Get<CmsApiClientOptions>() ?? new CmsApiClientOptions());
            services.AddHostedServiceTelemetryWrapper();

            var policyRegistry = services.AddPolicyRegistry();

            services.AddApiServices(configuration, policyRegistry);
            services.AddMvc(config =>
                {
                    config.RespectBrowserAcceptHeader = true;
                    config.ReturnHttpNotAcceptable = true;
                })
                .AddNewtonsoftJson()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        private void ConfigureMinimumThreads()
        {
            var workerThreads = Convert.ToInt32(configuration[WorkerThreadsConfigAppSettings]);

            var iocpThreads = Convert.ToInt32(configuration[IocpThreadsConfigAppSettings]);

            if (ThreadPool.SetMinThreads(workerThreads, iocpThreads))
            {
                logger.LogInformation(
                    "ConfigureMinimumThreads: Minimum configuration value set. IOCP = {0} and WORKER threads = {1}",
                    iocpThreads,
                    workerThreads);
            }
            else
            {
                logger.LogWarning("ConfigureMinimumThreads: The minimum number of threads was not changed");
            }
        }
    }
}