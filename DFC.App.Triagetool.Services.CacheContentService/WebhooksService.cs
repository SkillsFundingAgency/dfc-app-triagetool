using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Enums;
using DFC.App.Triagetool.Data.Helpers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Services.CacheContentService
{
    public class WebhooksService : IWebhooksService
    {
        private readonly ILogger<WebhooksService> logger;
        private readonly IEnumerable<IEventHandler> eventHandlers;

        public WebhooksService(
            ILogger<WebhooksService> logger,
            IEnumerable<IEventHandler> eventHandlers)
        {
            this.logger = logger;
            this.eventHandlers = eventHandlers;
        }

        public async Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint)
        {
            if (string.IsNullOrEmpty(apiEndpoint))
            {
                return HttpStatusCode.Accepted;
            }

            switch (webhookCacheOperation)
            {
                case WebhookCacheOperation.Delete:
                    return await GetMessageHandler(apiEndpoint).DeleteContentAsync(contentId).ConfigureAwait(false);

                case WebhookCacheOperation.CreateOrUpdate:
                    if (!Uri.TryCreate(apiEndpoint, UriKind.Absolute, out Uri? url))
                    {
                        throw new InvalidDataException($"Invalid Api url '{apiEndpoint}' received for Event Id: {eventId}");
                    }

                    return await GetMessageHandler(apiEndpoint).ProcessContentAsync(url).ConfigureAwait(false);

                default:
                    logger.LogError($"Event Id: {eventId} got unknown cache operation - {webhookCacheOperation}");
                    return HttpStatusCode.BadRequest;
            }
        }

        private IEventHandler GetMessageHandler(string apiEndpoint)
        {
            IEventHandler? handler;

            if (apiEndpoint.Contains(CmsContentKeyHelper.SharedContentTag, StringComparison.CurrentCultureIgnoreCase))
            {
                handler = eventHandlers.FirstOrDefault(x =>
                    x.ProcessType == DependencyInjectionKeyHelpers.SharedContentEventHandler);
                if (handler == null)
                {
                    throw new InvalidOperationException("No implementation for SharedContentEventHandler Found");
                }
            }
            else
            {
                handler = eventHandlers.FirstOrDefault(x => x.ProcessType == DependencyInjectionKeyHelpers.CmsEventHandler);
                if (handler == null)
                {
                    throw new InvalidOperationException("No implementation for CmsEventHandler Found");
                }
            }

            return handler;
        }
    }
}
