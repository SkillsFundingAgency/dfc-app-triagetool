﻿using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Enums;
using DFC.App.Triagetool.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Services.CacheContentService
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly ICmsApiService cmsApiService;
        private readonly IDocumentService<SharedContentItemModel> sharedContentItemDocumentService;

        public WebhooksService(
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

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint)
        {
            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    return await DeleteContentAsync(contentId).ConfigureAwait(false);

                case WebhookCacheOperation.CreateOrUpdate:
                    if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out Uri? url))
                    {
                        throw new InvalidDataException($"Invalid Api url '{apiEndpoint}' received for Event Id: {eventId}");
                    }

                    return await ProcessContentAsync(url).ConfigureAwait(false);

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

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
