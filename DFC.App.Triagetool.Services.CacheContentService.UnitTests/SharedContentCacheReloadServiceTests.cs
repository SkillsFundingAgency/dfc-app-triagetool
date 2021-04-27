using AutoMapper;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests
{
    public class SharedContentCacheReloadServiceTests
    {
        private readonly IMapper fakeMapper = A.Fake<IMapper>();
        private readonly IDocumentService<SharedContentItemModel> fakeSharedContentItemDocumentService = A.Fake<IDocumentService<SharedContentItemModel>>();
        private readonly ICmsApiService fakeCmsApiService = A.Fake<ICmsApiService>();

        [Fact]
        public async Task SharedContentCacheReloadServiceReloadAllCancellationRequestedCancels()
        {
            //Arrange
            var cancellationToken = new CancellationToken(true);
            var sharedContentCacheReloadService = new SharedContentCacheReloadService(A.Fake<ILogger<SharedContentCacheReloadService>>(), fakeMapper, fakeSharedContentItemDocumentService, fakeCmsApiService);

            //Act
            await sharedContentCacheReloadService.Reload(cancellationToken).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<SharedContentItemApiDataModel>(A<string>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeSharedContentItemDocumentService.UpsertAsync(A<SharedContentItemModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task SharedContentCacheReloadServiceReloadAllReloadsItems()
        {
            //Arrange
            var fakeContentItem = A.Dummy<SharedContentItemApiDataModel>();

            A.CallTo(() => fakeCmsApiService.GetItemAsync<SharedContentItemApiDataModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(fakeContentItem);
            var sharedContentCacheReloadService = new SharedContentCacheReloadService(A.Fake<ILogger<SharedContentCacheReloadService>>(), fakeMapper, fakeSharedContentItemDocumentService, fakeCmsApiService);

            //Act
            await sharedContentCacheReloadService.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<SharedContentItemApiDataModel>(A<string>.Ignored, A<Guid>.Ignored)).MustHaveHappened(SharedContentKeyHelper.GetSharedContentKeys().Count(), Times.Exactly);
            A.CallTo(() => fakeSharedContentItemDocumentService.UpsertAsync(A<SharedContentItemModel>.Ignored)).MustHaveHappened(SharedContentKeyHelper.GetSharedContentKeys().Count(), Times.Exactly);
        }
    }
}
