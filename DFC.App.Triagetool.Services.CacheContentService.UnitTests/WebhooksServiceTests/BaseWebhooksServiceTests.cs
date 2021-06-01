using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.App.Triagetool.Data.Models.ContentModels;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseWebhooksServiceTests
    {
        protected const string EventTypePublished = "published";
        protected const string EventTypeDraft = "draft";
        protected const string EventTypeDraftDiscarded = "draft-discarded";
        protected const string EventTypeDeleted = "deleted";
        protected const string EventTypeUnpublished = "unpublished";

        protected BaseWebhooksServiceTests()
        {
            Logger = A.Fake<ILogger<WebhooksService>>();
            FakeEventHandlers = new List<IEventHandler>();
        }

        protected Guid ContentIdForCreate { get; } = Guid.NewGuid();

        protected Guid ContentIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForDelete { get; } = Guid.NewGuid();

        protected ILogger<WebhooksService> Logger { get; }

        protected IList<IEventHandler> FakeEventHandlers { get; }

        protected static SharedContentItemApiDataModel BuildValidContentItemApiDataModel()
        {
            var model = new SharedContentItemApiDataModel
            {
                Title = "an-article",
                Url = new Uri("https://localhost"),
                Content = "some content",
                Published = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
            };

            return model;
        }

        protected SharedContentItemModel BuildValidContentItemModel()
        {
            var model = new SharedContentItemModel()
            {
                Id = ContentIdForUpdate,
                Etag = Guid.NewGuid().ToString(),
                Title = "an-article",
                Url = new Uri("https://localhost"),
                Content = "some content",
                LastReviewed = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                LastCached = DateTime.UtcNow,
            };

            return model;
        }

        protected WebhooksService BuildWebhooksService()
        {
            var service = new WebhooksService(Logger, FakeEventHandlers);

            return service;
        }

        protected IEventHandler AddSharedEventHandler()
        {
            var handler = A.Fake<IEventHandler>();
            FakeEventHandlers.Add(handler);
            A.CallTo(() => handler.ProcessType).Returns(DependencyInjectionKeyHelpers.SharedContentEventHandler);
            FakeEventHandlers.Add(handler);
            return handler;
        }
    }
}
