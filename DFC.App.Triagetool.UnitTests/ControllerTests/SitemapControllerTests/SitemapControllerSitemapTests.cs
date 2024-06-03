using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.Models;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using AppConstants = DFC.Common.SharedContent.Pkg.Netcore.Constant.ApplicationKeys;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.SitemapControllerTests
{
    [Trait("Category", "Sitemap Controller Unit Tests")]
    public class SitemapControllerSitemapTests
    {
        [Fact]
        public async Task SitemapControllerSitemapReturnsSuccess()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<SitemapController>>();
            var requestMock = new Mock<HttpRequest>();
            var httpContextMock = new Mock<HttpContext>();
            var contentMode = new Dictionary<string, string>
            {
                {"contentMode:contentMode", "PUBLISHED" },
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(contentMode)
                .Build();

            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

            var sharedContentRedisMock = new Mock<ISharedContentRedisInterface>();
            var triageResponse = new TriageToolFilterResponse
            {
                TriageToolFilter = new List<TriageToolFilters>
                {
                    new()
                    {
                        DisplayText = "Changing your career"
                    },
                },
            };
            sharedContentRedisMock.Setup(m => m.GetDataAsync<TriageToolFilterResponse>(AppConstants.TriageToolFilters, "PUBLISHED")).ReturnsAsync(triageResponse);
            var sitemap = new SitemapLocation
            {
                Url = triageResponse.TriageToolFilter[0].DisplayText,
                Priority = 0.5,
            };


            var controller = new SitemapController(loggerMock.Object, sharedContentRedisMock.Object, configuration);

            //Act
            var result = await controller.Sitemap();

            //Assert
            Assert.IsType<ContentResult>(result);
            var contentResult = Assert.IsType<ContentResult>(result);
            Assert.NotNull(contentResult.Content);
        }

        [Fact]
        public async Task SitemapControllerSitemapReturnsSuccessWhenNoData()
        {
            //Arrange
            var loggerMock = new Mock<ILogger<SitemapController>>();
            var requestMock = new Mock<HttpRequest>();
            var contentMode = new Dictionary<string, string>
            {
                {"contentMode:contentMode", "PUBLISHED" },
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(contentMode)
                .Build();

            requestMock.Setup(r => r.Scheme).Returns("https");
            requestMock.Setup(r => r.Host).Returns(new HostString("example.com"));
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

            var sharedContentRedisMock = new Mock<ISharedContentRedisInterface>();
            sharedContentRedisMock.Setup(m => m.GetDataAsync<TriageToolFilterResponse>(AppConstants.TriageToolFilters, "PUBLISHED")).ReturnsAsync((TriageToolFilterResponse) null);
            var controller = new SitemapController(loggerMock.Object, sharedContentRedisMock.Object, configuration);

            //Act
            var result = await controller.Sitemap();

            //Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
