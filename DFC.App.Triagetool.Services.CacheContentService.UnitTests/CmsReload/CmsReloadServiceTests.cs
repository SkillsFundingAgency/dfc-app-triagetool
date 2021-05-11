using AutoMapper;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.Compui.Cosmos.Contracts;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.CmsReload
{
    [ExcludeFromCodeCoverage]
    public class CmsReloadServiceTests : BaseCmsReloadTests
    {
        private readonly IMapper fakeMapper = A.Fake<IMapper>();
        private readonly IDocumentService<TriageToolOptionDocumentModel> fakeDocumentService = A.Fake<IDocumentService<TriageToolOptionDocumentModel>>();
        private readonly ICmsApiService fakeCmsApiService = A.Fake<ICmsApiService>();
        private readonly IContentTypeMappingService fakeContentTypeMappingService = A.Fake<IContentTypeMappingService>();

        [Fact]
        public async Task CacheReloadServiceReloadAllCancellationRequestedCancels()
        {
            //Arrange
            var cancellationToken = new CancellationToken(true);
            var Service = new CacheReloadService(A.Fake<ILogger<CacheReloadService>>(), fakeMapper, fakeDocumentService, fakeCmsApiService, fakeContentTypeMappingService);

            //Act
            await Service.Reload(cancellationToken).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionSummaryModel>(A<string>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeDocumentService.UpsertAsync(A<TriageToolOptionDocumentModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceReloadDoesNotGetPagesIfNoOptionsAreFound()
        {
            //Arrange
            var dummyContentItem = A.Dummy<TriageToolOptionSummaryModel>();

            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionSummaryModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(dummyContentItem);
            var Service = new CacheReloadService(A.Fake<ILogger<CacheReloadService>>(), fakeMapper, fakeDocumentService, fakeCmsApiService, fakeContentTypeMappingService);

            //Act
            await Service.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionItemModel>(A<string>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeDocumentService.UpsertAsync(A<TriageToolOptionDocumentModel>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeCmsApiService.GetSummaryAsync<CmsApiSummaryItemModel>(CmsContentKeyHelper.PageTag)).MustNotHaveHappened();
            A.CallTo(() => fakeCmsApiService.GetSummaryAsync<TriageToolOptionSummaryModel>(CmsContentKeyHelper.OptionTag)).MustHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceReloadAllReloadsItems()
        {
            //Arrange
            var options = GetValidCmsOptionSummary();

            A.CallTo(() =>
                    fakeCmsApiService.GetSummaryAsync<TriageToolOptionSummaryModel>(CmsContentKeyHelper.OptionTag))
                .Returns(options);
            A.CallTo(() =>
                    fakeCmsApiService.GetSummaryAsync<CmsApiSummaryItemModel>(CmsContentKeyHelper.PageTag))
                .Returns(GetValidPagesSummary());
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionItemModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(GetValidOptionItem());
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiDataModel>(A<Uri>.Ignored)).Returns(GetValidPage());
            var Service = new CacheReloadService(A.Fake<ILogger<CacheReloadService>>(), fakeMapper, fakeDocumentService, fakeCmsApiService, fakeContentTypeMappingService);
            A.CallTo(() => fakeMapper.Map<IList<PageDocumentModel>>(A<IList<CmsApiDataModel>>.Ignored))
                .Returns(new List<PageDocumentModel> { GetPageDocumentModel(), });
            A.CallTo(() => fakeMapper.Map<IList<TriageToolOptionDocumentModel>>(A<IList<TriageToolOptionItemModel>>.Ignored))
                .Returns(new List<TriageToolOptionDocumentModel> { GetTriageToolOptionDocumentModel(), });

            //Act
            await Service.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionItemModel>(A<Uri>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiDataModel>(A<Uri>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeDocumentService.UpsertAsync(A<TriageToolOptionDocumentModel>.Ignored)).MustHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceReloadWhenOptionHasNoPagesOptionIsStillSaved()
        {
            //Arrange
            var options = GetValidCmsOptionSummary();

            A.CallTo(() =>
                    fakeCmsApiService.GetSummaryAsync<TriageToolOptionSummaryModel>(CmsContentKeyHelper.OptionTag))
                .Returns(options);
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionItemModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(GetValidOptionItem());
            var Service = new CacheReloadService(A.Fake<ILogger<CacheReloadService>>(), fakeMapper, fakeDocumentService, fakeCmsApiService, fakeContentTypeMappingService);
            A.CallTo(() => fakeMapper.Map<IList<PageDocumentModel>>(A<IList<CmsApiDataModel>>.Ignored))
                .Returns(new List<PageDocumentModel> { GetPageDocumentModel(), });
            A.CallTo(() => fakeMapper.Map<IList<TriageToolOptionDocumentModel>>(A<IList<TriageToolOptionItemModel>>.Ignored))
                .Returns(new List<TriageToolOptionDocumentModel> { GetTriageToolOptionDocumentModel(), });

            //Act
            await Service.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionItemModel>(A<Uri>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiDataModel>(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeDocumentService.UpsertAsync(A<TriageToolOptionDocumentModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeCmsApiService.GetSummaryAsync<CmsApiSummaryItemModel>(CmsContentKeyHelper.PageTag)).MustHaveHappened();
            A.CallTo(() => fakeCmsApiService.GetSummaryAsync<TriageToolOptionSummaryModel>(CmsContentKeyHelper.OptionTag)).MustHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceReloadFailsLogError()
        {
            //Arrange
            var cancellationToken = new CancellationToken(true);
            var Service = new CacheReloadService(A.Fake<ILogger<CacheReloadService>>(), fakeMapper, fakeDocumentService, fakeCmsApiService, fakeContentTypeMappingService);

            A.CallTo(() =>
                    fakeCmsApiService.GetSummaryAsync<TriageToolOptionSummaryModel>(CmsContentKeyHelper.OptionTag))
                .Throws<Exception>();

            //Act
            await Service.Reload(cancellationToken).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionSummaryModel>(A<string>.Ignored, A<Guid>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => fakeDocumentService.UpsertAsync(A<TriageToolOptionDocumentModel>.Ignored)).MustNotHaveHappened();
        }

        [Fact]
        public async Task CacheReloadServiceReloadDeleteNoLongerFoundOptions()
        {
            //Arrange
            var options = GetValidCmsOptionSummary();

            A.CallTo(() =>
                    fakeCmsApiService.GetSummaryAsync<TriageToolOptionSummaryModel>(CmsContentKeyHelper.OptionTag))
                .Returns(options);
            A.CallTo(() =>
                    fakeCmsApiService.GetSummaryAsync<CmsApiSummaryItemModel>(CmsContentKeyHelper.PageTag))
                .Returns(GetValidPagesSummary());
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionItemModel>(A<string>.Ignored, A<Guid>.Ignored)).Returns(GetValidOptionItem());
            var Service = new CacheReloadService(A.Fake<ILogger<CacheReloadService>>(), fakeMapper, fakeDocumentService, fakeCmsApiService, fakeContentTypeMappingService);
            A.CallTo(() => fakeMapper.Map<IList<PageDocumentModel>>(A<IList<CmsApiDataModel>>.Ignored))
                .Returns(new List<PageDocumentModel> { GetPageDocumentModel(), });
            A.CallTo(() => fakeMapper.Map<IList<TriageToolOptionDocumentModel>>(A<IList<TriageToolOptionItemModel>>.Ignored))
                .Returns(new List<TriageToolOptionDocumentModel> { GetTriageToolOptionDocumentModel(), });
            A.CallTo(() => fakeDocumentService.GetAllAsync(A<string>.Ignored)).Returns(
                new List<TriageToolOptionDocumentModel>
                {
                    GetTriageToolOptionDocumentModel(uri: new Uri("http://tetsing.com")),
                });

            //Act
            await Service.Reload(CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => fakeCmsApiService.GetItemAsync<TriageToolOptionItemModel>(A<Uri>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeCmsApiService.GetItemAsync<CmsApiDataModel>(A<Uri>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeDocumentService.UpsertAsync(A<TriageToolOptionDocumentModel>.Ignored)).MustHaveHappened();
            A.CallTo(() => fakeDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappened();
        }
    }
}
