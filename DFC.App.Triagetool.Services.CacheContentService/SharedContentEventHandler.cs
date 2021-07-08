using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Services.CacheContentService
{
    public class SharedContentEventHandler : IEventHandler
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly ICmsApiService cmsApiService;
        private readonly IDocumentService<SharedContentItemModel> sharedContentItemDocumentService;

        public SharedContentEventHandler(
            ILogger<WebhooksService> logger,
            AutoMapper.IMapper mapper,
            ICmsApiService cmsApiService,
            IDocumentService<SharedContentItemModel> sharedContentItemDocumentService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.cmsApiService = cmsApiService;
            this.sharedContentItemDocumentService = sharedContentItemDocumentService;
        }

        public string ProcessType => DependencyInjectionKeyHelpers.SharedContentEventHandler;

        public async Task<HttpStatusCode> ProcessContentAsync(Uri url)
        {
            var apiDataModel = await cmsApiService.GetItemAsync<SharedContentItemApiDataModel>(url).ConfigureAwait(false);
            var sharedContentItemModel = mapper.Map<SharedContentItemModel>(apiDataModel);

            if (sharedContentItemModel == null)
            {
                return HttpStatusCode.NoContent;
            }

            if (!TryValidateModel(sharedContentItemModel))
            {
                return HttpStatusCode.BadRequest;
            }

            var contentResult = await sharedContentItemDocumentService.UpsertAsync(sharedContentItemModel).ConfigureAwait(false);

            return contentResult;
        }

        public async Task<HttpStatusCode> DeleteContentAsync(Guid contentId)
        {
            var result = await sharedContentItemDocumentService.DeleteAsync(contentId).ConfigureAwait(false);

            return result ? HttpStatusCode.OK : HttpStatusCode.NoContent;
        }

        public bool TryValidateModel(SharedContentItemModel? sharedContentItemModel)
        {
            _ = sharedContentItemModel ?? throw new ArgumentNullException(nameof(sharedContentItemModel));

            var validationContext = new ValidationContext(sharedContentItemModel, null, null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(sharedContentItemModel, validationContext, validationResults, true);

            if (!isValid && validationResults.Any())
            {
                foreach (var validationResult in validationResults)
                {
                    logger.LogError($"Error validating {sharedContentItemModel.Title} - {sharedContentItemModel.Url}: {string.Join(",", validationResult.MemberNames)} - {validationResult.ErrorMessage}");
                }
            }

            return isValid;
        }
    }
}
