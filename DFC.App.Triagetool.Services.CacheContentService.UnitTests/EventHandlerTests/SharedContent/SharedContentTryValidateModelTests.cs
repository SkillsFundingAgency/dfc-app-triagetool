using System;
using System.Diagnostics.CodeAnalysis;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.Services.CacheContentService.UnitTests.WebhooksServiceTests;
using Xunit;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.EventHandlerTests.SharedContent
{
    [Trait("Category", "SharedContent Event Handler TryValidateModel Unit Tests")]
    [ExcludeFromCodeCoverage]
    public class SharedContentTryValidateModelTests : BaseEventHandlerTests
    {
        [Fact]
        public void WebhooksServiceTryValidateModelForCreateReturnsSuccess()
        {
            // Arrange
            const bool expectedResponse = true;
            var expectedValidContentItemModel = BuildValidContentItemModel();
            var handler = BuildSharedContentEventHandler();

            // Act
            var result = handler.TryValidateModel(expectedValidContentItemModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void WebhooksServiceTryValidateModelForUpdateReturnsFailure()
        {
            // Arrange
            const bool expectedResponse = false;
            var expectedInvalidContentItemModel = new SharedContentItemModel();
            var handler = BuildSharedContentEventHandler();

            // Act
            var result = handler.TryValidateModel(expectedInvalidContentItemModel);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void WebhooksServiceTryValidateModelRaisesExceptionForNullContentItemModel()
        {
            // Arrange
            SharedContentItemModel? nullContentItemModel = null;
            var handler = BuildSharedContentEventHandler();

            // Act
            var exceptionResult = Assert.Throws<ArgumentNullException>(() => handler.TryValidateModel(nullContentItemModel));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'sharedContentItemModel')", exceptionResult.Message);
        }
    }
}
