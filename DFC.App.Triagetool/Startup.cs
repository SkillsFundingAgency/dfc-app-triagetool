using AutoMapper;
using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.HostedServices;
using DFC.App.Triagetool.Services.CacheContentService;
using DFC.Compui.Cosmos;
using DFC.Compui.Cosmos.Contracts;
using DFC.Compui.Subscriptions.Pkg.Netstandard.Extensions;
using DFC.Compui.Telemetry;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using DFC.Content.Pkg.Netcore.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        private const string CosmosDbSharedContentConfigAppSettings = "Configuration:CosmosDbConnections:SharedContent";
        private const string CosmosDbCmsContentConfigAppSettings = "Configuration:CosmosDbConnections:TriageTool";

        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment env;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            this.configuration = configuration;
            this.env = env;
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IMapper mapper)
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
            var cosmosDbConnectionSharedContent = configuration.GetSection(CosmosDbSharedContentConfigAppSettings).Get<CosmosDbConnection>();
            var cosmosDbConnectionCmsContent = configuration.GetSection(CosmosDbCmsContentConfigAppSettings).Get<CosmosDbConnection>();
            services.AddDocumentServices<SharedContentItemModel>(cosmosDbConnectionSharedContent, env.IsDevelopment());
            services.AddDocumentServices<TriageToolOptionDocumentModel>(cosmosDbConnectionCmsContent, env.IsDevelopment());

            services.AddApplicationInsightsTelemetry();
            services.AddHttpContextAccessor();
            services.AddTransient<ISharedContentCacheReloadService, SharedContentCacheReloadService>();
            services.AddTransient<IWebhooksService, WebhooksService>();
            services.AddTransient<ICacheReloadService, CacheReloadService>();
            services.AddTransient<IEventHandler, SharedContentEventHandler>();
            services.AddTransient<IEventHandler, CmsEventHandler>();

            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSingleton(configuration.GetSection(nameof(CmsApiClientOptions)).Get<CmsApiClientOptions>() ?? new CmsApiClientOptions());
            services.AddHostedServiceTelemetryWrapper();
            services.AddHostedService<SharedContentCacheReloadBackgroundService>();
            services.AddHostedService<ContentCacheReloadBackgroundService>();
            services.AddSubscriptionBackgroundService(configuration);

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
    }
}