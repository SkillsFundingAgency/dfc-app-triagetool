using DFC.App.Triagetool.Data.Models.ContentModels;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.SitemapControllerTests
{
    [Trait("Category", "Sitemap Controller Unit Tests")]
    public class SitemapControllerViewTests : BaseSitemapControllerTests
    {
        [Fact]
        public async Task SitemapControllerViewReturnsSuccess()
        {
            // Arrange
            const int resultsCount = 2;
            var expectedResults = A.CollectionOfFake<TriageToolOptionDocumentModel>(resultsCount);
            using var controller = BuildSitemapController();

            A.CallTo(() => FakeTriageToolDocumentService.GetAllAsync(A<string>.Ignored)).Returns(expectedResults);

            // Act
            var result = await controller.SitemapView().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTriageToolDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            var contentResult = Assert.IsType<ContentResult>(result);

            contentResult.ContentType.Should().Be(MediaTypeNames.Application.Xml);
        }

        [Fact]
        public async Task SitemapControllerViewReturnsSuccessWhenNoData()
        {
            // Arrange
            const int resultsCount = 0;
            var expectedResults = A.CollectionOfFake<TriageToolOptionDocumentModel>(resultsCount);
            using var controller = BuildSitemapController();

            A.CallTo(() => FakeTriageToolDocumentService.GetAllAsync(A<string>.Ignored)).Returns(expectedResults);

            // Act
            var result = await controller.SitemapView().ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTriageToolDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();

            _ = Assert.IsType<NoContentResult>(result);
        }
    }
}
