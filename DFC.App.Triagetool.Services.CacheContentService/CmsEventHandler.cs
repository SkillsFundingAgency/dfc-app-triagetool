using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Services.CacheContentService
{
    public class CmsEventHandler : IEventHandler
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly ICacheReloadService contentCacheReloadService;

        public CmsEventHandler(
            ILogger<WebhooksService> logger,
            ICacheReloadService contentCacheReloadService)
        {
            this.logger = logger;
            this.contentCacheReloadService = contentCacheReloadService;
        }

        public string ProcessType => DependencyInjectionKeyHelpers.CmsEventHandler;

        public async Task<HttpStatusCode> ProcessContentAsync(Uri url)
        {
            return await ReloadContent().ConfigureAwait(false);
        }

        public async Task<HttpStatusCode> DeleteContentAsync(Guid contentId)
        {
            return await ReloadContent().ConfigureAwait(false);
        }

        private async Task<HttpStatusCode> ReloadContent()
        {
            logger.LogInformation("Reloading All content");
            await contentCacheReloadService.Reload(new CancellationToken(false)).ConfigureAwait(false);
            return HttpStatusCode.OK;
        }
    }
}
