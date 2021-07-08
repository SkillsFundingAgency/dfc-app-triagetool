using DFC.App.Triagetool.Data.Enums;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Data.Contracts
{
    public interface IWebhooksService
    {
        Task<HttpStatusCode> ProcessMessageAsync(WebhookCacheOperation webhookCacheOperation, Guid eventId, Guid contentId, string apiEndpoint);
    }
}
