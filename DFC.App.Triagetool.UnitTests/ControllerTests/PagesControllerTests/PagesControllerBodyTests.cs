using AutoMapper;
using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller - Body Unit Tests")]
    public class PagesControllerBodyTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBodyReturnsViewWhenNoDataFound(string mediaTypeName)
        {
            var contentMode = new Dictionary<string, string>
            {
                {"contentMode:contentMode", "PUBLISHED" },
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(contentMode)
                .Build();

            var redisMock = new Mock<ISharedContentRedisInterface>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PagesController>>();

            redisMock.Setup(r => r.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All", "PUBLISHED", 4))
                     .ReturnsAsync((TriageToolFilterResponse)null);

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);

            // Act
            var result = await controller.Body("YourArticleId","Article") as ViewResult;

            // Assert
            Assert.Equal(null, result.ContentType);

        }

        [Fact]
        public async Task PagesControllerBodyCalledWithValueReturnsCorrectView()
        {
            var redisMock = new Mock<ISharedContentRedisInterface>();
            var mapperMock = new Mock<IMapper>();
            var contentMode = new Dictionary<string, string>
            {
                {"contentMode:contentMode", "PUBLISHED" },
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(contentMode)
                .Build();

            var loggerMock = new Mock<ILogger<PagesController>>();
            var triageToolFilterResponse = new TriageToolFilterResponse
            {
                TriageToolFilter = new List<TriageToolFilters>
            {
                new () { DisplayText = "Option1" },
                new () { DisplayText = "Option2" },
            },
            };
            redisMock.Setup(r => r.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All", "PUBLISHED", 4))
                     .ReturnsAsync(triageToolFilterResponse);

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);

            var mockSharedContentRedis = new Mock<ISharedContentRedisInterface>();
            var triagePageResponse = new TriagePageResponse();
            mockSharedContentRedis.Setup(x => x.GetDataAsync<TriagePageResponse>("TriageToolPages", "PUBLISHED", 4)).ReturnsAsync(triagePageResponse);

            // Act
            var result = await controller.Body("Article", "Article");

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
            //Assert.IsInstanceOf<TriageToolOptionViewModel>(viewResult.Model);
            var model = viewResult.Model as TriageToolOptionViewModel;
            Assert.Equal("Article", model.Title);

        }

        [Fact]
        public async Task PagesControllerBodyReturnsFirstDocumentWhenCalledWithOutOption()
        {
            var redisMock = new Mock<ISharedContentRedisInterface>();
            var mapperMock = new Mock<IMapper>();
            var contentMode = new Dictionary<string, string>
            {
                {"contentMode:contentMode", "PUBLISHED" },
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(contentMode)
                .Build();

            var loggerMock = new Mock<ILogger<PagesController>>();
            var triageToolFilterResponse = new TriageToolFilterResponse
            {
                TriageToolFilter = new List<TriageToolFilters>
            {
                new () { DisplayText = "Option1" },
                new () { DisplayText = "Option2" },
            },
            };
            redisMock.Setup(r => r.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All", "PUBLISHED", 4))
                     .ReturnsAsync(triageToolFilterResponse);

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);

            // Mocking sharedContentRedis.GetDataAsync<TriagePageResponse>
            var mockSharedContentRedis = new Mock<ISharedContentRedisInterface>();
            var triagePageResponse = new TriagePageResponse();
            mockSharedContentRedis.Setup(x => x.GetDataAsync<TriagePageResponse>("TriageToolPages", "PUBLISHED", 4)).ReturnsAsync(triagePageResponse);

            // Act
            var result = await controller.Body("Article", "Article");

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        private TriageLookupResponse GetTriageLookupResponse()
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
                new TriageLevelOne
                {
                    ContentItemId = "2",
                    Title = "Employed",
                    Ordinal = 2,
                    Value = "Employed",
                    LevelTwo = new TriageLevelTwo
                    {
                        ContentItems = new List<TriageLevelTwo>
                        {
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo4"
                                },
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo5"
                                },
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo6"
                                },
                        },
                    },
                },
                new TriageLevelOne
                {
                    ContentItemId = "3",
                    Title = "Not In Work",
                    Ordinal = 1,
                    Value = "not in work",
                    LevelTwo = new TriageLevelTwo
                    {
                        ContentItems = new List<TriageLevelTwo>
                        {
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo7"
                                },
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo8"
                                },
                                new TriageLevelTwo
                                {
                                     ContentItemId = "LevelTwo9"
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
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo4",
                    Title = "LevelTwo4",
                    Value = "LevelTwo4",
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
                    ContentItemId = "LevelTwo5",
                    Title = "LevelTwo5",
                    Value = "LevelTwo5",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "1" },
                            new FilterAdviceGroup { ContentItemId = "4" },
                        },
                    },
                },
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo6",
                    Title = "LevelTwo6",
                    Value = "LevelTwo6",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "1" },
                            new FilterAdviceGroup { ContentItemId = "2" },
                            new FilterAdviceGroup { ContentItemId = "3" },
                            new FilterAdviceGroup { ContentItemId = "4" },
                        },
                    },
                },
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo7",
                    Title = "LevelTwo7",
                    Value = "LevelTwo7",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "3" },
                            new FilterAdviceGroup { ContentItemId = "4" },
                        },
                    },
                },
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo8",
                    Title = "LevelTwo8",
                    Value = "LevelTwo8",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "1" },
                            new FilterAdviceGroup { ContentItemId = "2" },
                            new FilterAdviceGroup { ContentItemId = "3" },
                            new FilterAdviceGroup { ContentItemId = "4" },
                        },
                    },
                },
                new TriageLevelTwo
                {
                    ContentItemId = "LevelTwo9",
                    Title = "LevelTwo9",
                    Value = "LevelTwo9",
                    FilterAdviceGroup = new FilterAdviceGroup
                    {
                        ContentItems = new List<FilterAdviceGroup>
                        {
                            new FilterAdviceGroup { ContentItemId = "1" },
                            new FilterAdviceGroup { ContentItemId = "2" },
                            new FilterAdviceGroup { ContentItemId = "3" },
                            new FilterAdviceGroup { ContentItemId = "4" },
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

        private TriageResultPageResponse GetTriagePageResponse()
        {
            var response = new TriageResultPageResponse
            {
                Page = new List<TriageResultPage>
                {
                   new TriageResultPage
                   {
                       TriageOrdinal = 3,
                       DisplayText = "page 1",
                       TriageLevelOne = new TriageLevelOne
                       {
                           ContentItems = new List<TriageLevelOne>
                           {
                               new TriageLevelOne
                               {
                                   ContentItemId = "1",
                               },
                           },
                       },
                       TriageLevelTwo = new TriageLevelTwo
                       {
                           ContentItems = new List<TriageLevelTwo>
                           {
                               new TriageLevelTwo
                               {
                                   ContentItemId = "LevelTwo1",
                               },
                           },
                       },
                   },
                   new TriageResultPage
                   {
                       TriageOrdinal = 2,
                       DisplayText = "page 2",
                       TriageLevelOne = new TriageLevelOne
                       {
                           ContentItems = new List<TriageLevelOne>
                           {
                               new TriageLevelOne
                               {
                                   ContentItemId = "1",
                               },
                           },
                       },
                   },
                },
            };

            return response;
        }
    }
}