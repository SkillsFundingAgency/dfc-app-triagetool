using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.Models;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
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
            var triageResponse = GetTriageLookupResponse();
            sharedContentRedisMock.Setup(m => m.GetDataAsyncWithExpiry<TriageLookupResponse>(AppConstants.TriageToolLookup, "PUBLISHED", 4)).ReturnsAsync(triageResponse);

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
            var controller = new SitemapController(loggerMock.Object, sharedContentRedisMock.Object, configuration);

            //Act
            var result = await controller.Sitemap();

            //Assert
            Assert.IsType<NoContentResult>(result);
        }

        private static TriageLookupResponse? GetTriageLookupResponse()
        {
            var response = new TriageLookupResponse();
            response.TriageLevelOne = new List<TriageLevelOne>
            {
                new TriageLevelOne
                {
                    ContentItemId = "1",
                    Title = "Education",
                    Ordinal = 3,
                    Value = "Education",
                    LevelTwo = new TriageLevelTwo
                    {
                        ContentItems = new List<TriageLevelTwo>
                        {
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo1"
                                },
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo2"
                                },
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo3"
                                },
                        },
                    },
                },
            };
            response.TriageLevelTwo = new List<TriageLevelTwo>
            {
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo1",
                    Title = "LevelTwo1",
                    Value = "LevelTwo1",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "1" },
                            new FilterAdviceGroup { ContentItemId = "2" },
                        },
                    },
                },
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo2",
                    Title = "LevelTwo2",
                    Value = "LevelTwo2",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "1" },
                            new FilterAdviceGroup { ContentItemId = "3" },
                        },
                    },
                },
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo3",
                    Title = "LevelTwo3",
                    Value = "LevelTwo3",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "1" },
                            new FilterAdviceGroup { ContentItemId = "2" },
                        },
                    },
                },
            };
            response.FilterAdviceGroup = new List<FilterAdviceGroup>
            {
                new FilterAdviceGroup { ContentItemId = "1", Title = "CV"},
                new FilterAdviceGroup { ContentItemId = "2", Title = "Options to work" },
                new FilterAdviceGroup { ContentItemId = "3", Title = "Interview tips" },
                new FilterAdviceGroup { ContentItemId = "4", Title = "Support from others" },
            };
            return response;
        }
    }
}
