using DFC.App.Triagetool.Data.Common;
using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models;
using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DFC.App.Triagetool.Data.Models.ContentModels;

namespace DFC.App.Triagetool.Services.CacheContentService
{
    public class CacheReloadService : ICacheReloadService
    {
        private readonly ILogger<CacheReloadService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IDocumentService<TriageToolOptionDocumentModel> cmsContentDocumentService;
        private readonly ICmsApiService cmsApiService;
        private readonly IContentTypeMappingService contentTypeMappingService;

        public CacheReloadService(ILogger<CacheReloadService> logger, AutoMapper.IMapper mapper, IDocumentService<TriageToolOptionDocumentModel> cmsContentDocumentService, ICmsApiService cmsApiService, IContentTypeMappingService contentTypeMappingService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.cmsContentDocumentService = cmsContentDocumentService;
            this.cmsApiService = cmsApiService;
            this.contentTypeMappingService = contentTypeMappingService;
        }

        public async Task Reload(CancellationToken stoppingToken)
        {
            try
            {
                logger.LogInformation("Reload cache started");

                contentTypeMappingService.AddMapping(Constants.ContentTypeTriageToolFilter, typeof(CmsTriageToolFilterModel));

                var options = await GetTriageToolOptions().ConfigureAwait(false);

                if (options.Any())
                {
                    var pages = await GetTriagePages().ConfigureAwait(false);

                    var documents = GetDocumentOptionModel(options, pages);

                    await UpdateDocuments(documents).ConfigureAwait(false);
                }

                logger.LogInformation("Reload cache completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unable to reload Triage Tool content from the CMS");
            }
        }

        private async Task<IList<TriageToolOptionItemModel>> GetTriageToolOptions()
        {
            var triageToolOptions = new List<TriageToolOptionItemModel>();
            var optionSummary = await cmsApiService.GetSummaryAsync<TriageToolOptionSummaryModel>(CmsContentKeyHelper.OptionTag)
                .ConfigureAwait(false);

            if (optionSummary != null && optionSummary.Any())
            {
                foreach (var option in optionSummary)
                {
                    var triageToolOption = await cmsApiService.GetItemAsync<TriageToolOptionItemModel>(option.Url!)
                        .ConfigureAwait(false);
                    if (triageToolOption != null)
                    {
                        triageToolOptions.Add(triageToolOption);
                    }
                }
            }

            return triageToolOptions;
        }

        private async Task<IList<CmsApiDataModel>> GetTriagePages()
        {
            var pages = new List<CmsApiDataModel>();

            var pagesSummary = await cmsApiService.GetSummaryAsync<CmsApiSummaryItemModel>(CmsContentKeyHelper.PageTag)
                .ConfigureAwait(false);

            if (pagesSummary != null && pagesSummary.Any())
            {
                foreach (var pageItem in pagesSummary)
                {
                    var page = await cmsApiService.GetItemAsync<CmsApiDataModel>(pageItem.Url!).ConfigureAwait(false);
                    if (page != null && page.UseInTriageTool)
                    {
                        pages.Add(page);
                    }
                }
            }

            return pages;
        }

        private IList<TriageToolOptionDocumentModel> GetDocumentOptionModel(IList<TriageToolOptionItemModel> options, IList<CmsApiDataModel> pages)
        {
            var pageDocuments = mapper.Map<IList<PageDocumentModel>>(pages);
            var optionsDocuments = mapper.Map<IList<TriageToolOptionDocumentModel>>(options);

            foreach (var optionDocument in optionsDocuments)
            {
                var pagesForOption = pageDocuments.Where(pd => pd.Uri != null && optionDocument.FilterIds.Any(od => pd.Filters.Contains(od))).ToList();

                if (pagesForOption.Any())
                {
                    optionDocument.Pages = pagesForOption.ToList();
                    optionDocument.PageIds = pagesForOption.Select(s => s.Uri!.ToString()).ToList();
                }
            }

            return optionsDocuments;
        }

        private async Task UpdateDocuments(IList<TriageToolOptionDocumentModel> options)
        {
            var currentOptions = await cmsContentDocumentService.GetAllAsync().ConfigureAwait(false);
            var existingDocuments = currentOptions?.ToList();
            if (existingDocuments != null)
            {
                var optionsToDelete = existingDocuments.Where(co => !options.Select(s => s.Url).ToList().Contains(co.Url));

                foreach (var optionToDelete in optionsToDelete)
                {
                    logger.LogInformation("Deleting options no longer on the content API");
                    await cmsContentDocumentService.DeleteAsync(optionToDelete.Id).ConfigureAwait(false);
                }
            }

            foreach (var option in options)
            {
                var existingDocument = existingDocuments?.FirstOrDefault(tto => tto.Url == option.Url);
                option.Id = existingDocument?.Id ?? Guid.NewGuid();
                logger.LogInformation("Upserting options");
                await cmsContentDocumentService.UpsertAsync(option).ConfigureAwait(false);
            }
        }
    }
}
