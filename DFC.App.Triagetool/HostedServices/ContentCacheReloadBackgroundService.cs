using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Services.CacheContentService;
using DFC.Compui.Telemetry.HostedService;
using DFC.Content.Pkg.Netcore.Data.Models.ClientOptions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.HostedServices
{
    [ExcludeFromCodeCoverage]
    public class ContentCacheReloadBackgroundService : BackgroundService
    {
        private readonly ILogger<ContentCacheReloadBackgroundService> logger;
        private readonly CmsApiClientOptions cmsApiClientOptions;
        private readonly ICacheReloadService contentCacheReloadBackgroundService;
        private readonly IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper;

        public ContentCacheReloadBackgroundService(ILogger<ContentCacheReloadBackgroundService> logger, CmsApiClientOptions cmsApiClientOptions, ICacheReloadService contentCacheReloadBackgroundService, IHostedServiceTelemetryWrapper hostedServiceTelemetryWrapper)
        {
            this.logger = logger;
            this.cmsApiClientOptions = cmsApiClientOptions;
            this.hostedServiceTelemetryWrapper = hostedServiceTelemetryWrapper;
            this.contentCacheReloadBackgroundService = contentCacheReloadBackgroundService;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Cache reload started");

            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Cache reload stopped");

            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                if (cmsApiClientOptions.BaseAddress == null)
                {
                    logger.LogInformation($"CMS Api Client Base Address is null, skipping Cache Reload");
                }

                logger.LogInformation($"Executing Telemetry wrapper with service {nameof(contentCacheReloadBackgroundService)}");

                var cacheReloadServiceTask = hostedServiceTelemetryWrapper.Execute(async () => await contentCacheReloadBackgroundService.Reload(stoppingToken).ConfigureAwait(false), nameof(CacheReloadService));
                await cacheReloadServiceTask.ConfigureAwait(false);

                //Caters for errors in the telemetry wrapper
                if (!cacheReloadServiceTask.IsCompletedSuccessfully)
                {
                    logger.LogInformation($"An error occurred in the {nameof(hostedServiceTelemetryWrapper)}");

                    if (cacheReloadServiceTask.Exception != null)
                    {
                        logger.LogError(cacheReloadServiceTask.Exception.ToString());
                        throw cacheReloadServiceTask.Exception;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
