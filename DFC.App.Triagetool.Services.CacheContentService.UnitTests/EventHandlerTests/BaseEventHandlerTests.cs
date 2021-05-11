using System;
using System.Diagnostics.CodeAnalysis;
using DFC.App.Triagetool.Data.Contracts;
using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.EventHandlerTests
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseEventHandlerTests
    {
        protected BaseEventHandlerTests()
        {
            Logger = A.Fake<ILogger<WebhooksService>>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeCmsApiService = A.Fake<ICmsApiService>();
            FakeSharedContentItemDocumentService = A.Fake<IDocumentService<SharedContentItemModel>>();
            FakeReloadService = A.Fake<ICacheReloadService>();
        }


        protected Guid ContentIdForUpdate { get; } = Guid.NewGuid();

        protected Guid ContentIdForDelete { get; } = Guid.NewGuid();

        protected ILogger<WebhooksService> Logger { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected ICmsApiService FakeCmsApiService { get; }

        protected IDocumentService<SharedContentItemModel> FakeSharedContentItemDocumentService { get; }

        protected ICacheReloadService FakeReloadService { get; set; }

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

        protected SharedContentEventHandler BuildSharedContentEventHandler()
        {
            var handler = new SharedContentEventHandler(Logger, FakeMapper, FakeCmsApiService, FakeSharedContentItemDocumentService);

            return handler;
        }

        protected CmsEventHandler BuildCmsEventHandler()
        {
            var handler = new CmsEventHandler(Logger, FakeReloadService);

            return handler;
        }
    }
}
