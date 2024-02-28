using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.SitemapControllerTests
{
    [Trait("Category", "Sitemap Controller Unit Tests")]
    public class SitemapControllerViewTests : BaseSitemapControllerTests
    {
        [Fact]
        public async Task Sitemap_ReturnsNoContent_When_TriageToolFilterResponseIsNull()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SitemapController>>();
            var requestMock = new Mock<HttpRequest>();
            requestMock.Setup(r => r.Scheme).Returns("https");
            requestMock.Setup(r => r.Host).Returns(new HostString("example.com"));
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

            var sharedContentRedisMock = new Mock<ISharedContentRedisInterface>();
            sharedContentRedisMock.Setup(m => m.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All")).ReturnsAsync((TriageToolFilterResponse)null);

            var controller = new SitemapController(loggerMock.Object, sharedContentRedisMock.Object);

            // Act
            var result = await controller.Sitemap();

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Sitemap_ReturnsContent_When_TriageToolFilterResponseIsNotEmpty()
        {
            // Arrange
            var loggerMock = new Mock<ILogger<SitemapController>>();

            var requestMock = new Mock<HttpRequest>();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

            var sharedContentRedisMock = new Mock<ISharedContentRedisInterface>();
            var triageToolFilterResponse = new TriageToolFilterResponse
            {
                TriageToolFilter = new List<TriageToolFilters>
                {
                      new() { DisplayText = "Test Page 1" },
                },

            };
            sharedContentRedisMock.Setup(m => m.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All")).ReturnsAsync(triageToolFilterResponse);

            var controller = new SitemapController(loggerMock.Object, sharedContentRedisMock.Object);

            // Act
            var result = await controller.Sitemap();

            // Assert
            Assert.IsType<ContentResult>(result);
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.NotNull(contentResult.Content);
            // Add more assertions as needed based on the expected content
        }

    }
}
