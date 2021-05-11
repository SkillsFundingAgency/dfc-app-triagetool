using DFC.App.Triagetool.Services.CacheContentService.UnitTests.WebhooksServiceTests;
using FakeItEasy;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.EventHandlerTests.SharedContent
{
    [Trait("Category", "SharedContent Event Handler DeleteContentAsync Unit Tests")]
    [ExcludeFromCodeCoverage]
    public class WebhooksServiceDeleteContentAsyncTests : BaseEventHandlerTests
    {
        [Fact]
        public async Task WebhooksServiceDeleteContentAsyncForCreateReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            const HttpStatusCode expectedResult = HttpStatusCode.OK;
            var service = BuildSharedContentEventHandler();

            A.CallTo(() => FakeSharedContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteContentAsync(ContentIdForDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSharedContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task WebhooksServiceDeleteContentAsyncForCreateReturnsNoContent()
        {
            // Arrange
            const bool expectedResponse = false;
            const HttpStatusCode expectedResult = HttpStatusCode.NoContent;
            var service = BuildSharedContentEventHandler();

            A.CallTo(() => FakeSharedContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).Returns(expectedResponse);

            // Act
            var result = await service.DeleteContentAsync(ContentIdForDelete).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeSharedContentItemDocumentService.DeleteAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResult, result);
        }
    }
}
