using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Services.CacheContentService
{
    public class SharedContentCacheReloadService : ISharedContentCacheReloadService
    {
        private readonly ILogger<SharedContentCacheReloadService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IDocumentService<SharedContentItemModel> sharedContentDocumentService;
        private readonly ICmsApiService cmsApiService;

        public SharedContentCacheReloadService(ILogger<SharedContentCacheReloadService> logger, AutoMapper.IMapper mapper, IDocumentService<SharedContentItemModel> sharedContentDocumentService, ICmsApiService cmsApiService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.sharedContentDocumentService = sharedContentDocumentService;
            this.cmsApiService = cmsApiService;
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Reload All content item cache started");

                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload content item cache cancelled");

                    return;
                }

                await ReloadSharedContent(stoppingToken).ConfigureAwait(false);

                logger.LogInformation("Reload All content item cache completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in content item cache reload");
                throw;
            }
        }

        public async Task ReloadSharedContent(CancellationToken stoppingToken)
        {
            var contentItemKeys = SharedContentKeyHelper.GetSharedContentKeys();

            foreach (var key in contentItemKeys)
            {
                if (stoppingToken.IsCancellationRequested)
                {
                    logger.LogWarning("Reload content item cache cancelled");

                    return;
                }

                var apiDataModel = await cmsApiService.GetItemAsync<SharedContentItemApiDataModel>("sharedcontent", key).ConfigureAwait(false);

                if (apiDataModel == null)
                {
                    logger.LogError($"Content item: {key} not found in API response");
                }

                //Add the e-mail to cache
                var mappedContentItem = mapper.Map<SharedContentItemModel>(apiDataModel);

                await sharedContentDocumentService.UpsertAsync(mappedContentItem).ConfigureAwait(false);
            }
        }
    }
}
