using DFC.App.Triagetool.Data.Enums;
using FakeItEasy;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.WebhooksServiceTests
{
    [Trait("Category", "Webhooks Service ProcessMessageAsync Unit Tests")]
    [ExcludeFromCodeCoverage]
    public class WebhooksServiceProcessMessageTests : BaseWebhooksServiceTests
    {
        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncNoneOptionReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.BadRequest;
            var url = "https://somewhere.com/SharedContent";
            var service = BuildWebhooksService();
            var handler = AddSharedEventHandler();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.None, Guid.NewGuid(), ContentIdForCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => handler.ProcessContentAsync(A<Uri>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncNullUriReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Accepted;
            var url = "https://somewhere.com/SharedContent";
            var service = BuildWebhooksService();
            var handler = AddSharedEventHandler();

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.None, Guid.NewGuid(), ContentIdForCreate, null).ConfigureAwait(false);

            // Assert
            A.CallTo(() => handler.ProcessContentAsync(A<Uri>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentCreateReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.Created;
            var url = "https://somewhere.com/SharedContent";
            var service = BuildWebhooksService();
            var handler = AddSharedEventHandler();

            A.CallTo(() => handler.ProcessContentAsync(A<Uri>.Ignored)).Returns(HttpStatusCode.Created);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForCreate, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => handler.ProcessContentAsync(A<Uri>.Ignored)).MustHaveHappenedOnceExactly();
            A.CallTo(() => handler.DeleteContentAsync(A<Guid>.Ignored)).MustNotHaveHappened();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentDeleteReturnsSuccess()
        {
            // Arrange
            const HttpStatusCode expectedResponse = HttpStatusCode.OK;
            var url = "https://somewhere.com/SharedContent";
            var service = BuildWebhooksService();
            var handler = AddSharedEventHandler();

            A.CallTo(() => handler.DeleteContentAsync(A<Guid>.Ignored)).Returns(HttpStatusCode.OK);

            // Act
            var result = await service.ProcessMessageAsync(WebhookCacheOperation.Delete, Guid.NewGuid(), ContentIdForDelete, url).ConfigureAwait(false);

            // Assert
            A.CallTo(() => handler.ProcessContentAsync(A<Uri>.Ignored)).MustNotHaveHappened();
            A.CallTo(() => handler.DeleteContentAsync(A<Guid>.Ignored)).MustHaveHappenedOnceExactly();

            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentThrowsErrorForInvalidUrl()
        {
            // Arrange
            var url = "/somewhere.com";
            var service = BuildWebhooksService();
            AddSharedEventHandler();
            // Act
            await Assert.ThrowsAsync<InvalidDataException>(async () => await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForCreate, url).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentThrowsErrorForUnknownHandler()
        {
            // Arrange
            var url = "https://somewhere.com/SharedContent";
            var service = BuildWebhooksService();
            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForCreate, url).ConfigureAwait(false)).ConfigureAwait(false);

        }

        [Fact]
        public async Task WebhooksServiceProcessMessageAsyncContentThrowsErrorForUnknownCmsHandler()
        {
            // Arrange
            var url = "https://somewhere.com/";
            var service = BuildWebhooksService();
            // Act
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await service.ProcessMessageAsync(WebhookCacheOperation.CreateOrUpdate, Guid.NewGuid(), ContentIdForCreate, url).ConfigureAwait(false)).ConfigureAwait(false);

        }
    }
}
