using DFC.App.Triagetool.Data.Helpers;
using FakeItEasy;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.EventHandlerTests.Cms
{
    [Trait("Category", "Cms Event Handler ProcessContentAsync Unit Tests")]
    [ExcludeFromCodeCoverage]
    public class CmsEventHandlerTests : BaseEventHandlerTests
    {
        [Fact]
        public async Task CmsEventHandlerProcessContentAsyncCallsReloadReturnsSuccessful()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = new Uri("https://somewhere.com");
            var service = BuildCmsEventHandler();

            // Act
            var result = await service.ProcessContentAsync(url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeReloadService.Reload(A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task CmsEventHandlerDeleteContentAsyncCallsReloadReturnsSuccessful()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var service = BuildCmsEventHandler();

            // Act
            var result = await service.DeleteContentAsync(Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeReloadService.Reload(A<CancellationToken>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void CmsEventHandlerProcessTypeReturnsCorrectValue()
        {
            // Arrange
            var service = BuildCmsEventHandler();

            Assert.Equal(service.ProcessType, DependencyInjectionKeyHelpers.CmsEventHandler);
        }
    }
}
