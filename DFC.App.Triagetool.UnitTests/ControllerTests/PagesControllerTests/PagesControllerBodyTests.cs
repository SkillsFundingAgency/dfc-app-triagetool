using AutoMapper;
using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller - Body Unit Tests")]
    public class PagesControllerBodyTests : BasePagesControllerTests
    {
        public static IEnumerable<object[]> FilterTestData => new List<object[]>
        {
            new object[] { "Education", "LevelTwo1" },
        };

        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBodyReturnsViewWhenNoDataFound(string mediaTypeName)
        {
            IConfiguration configuration = SetupConfiguration();

            Mock<ISharedContentRedisInterface> redisMock;
            PagesController controller;
            SetupController(configuration, out redisMock, out controller);

            // Act
            var result = await controller.Body("YourArticleId", "Article", null) as ViewResult;

            // Assert
            Assert.Equal(null, result.ContentType);
            redisMock.VerifyAll();
        }

        [Fact]
        public async Task PagesControllerBodyCalledWithNullFactorValuesShoutReturnNotFound()
        {
            IConfiguration configuration = SetupConfiguration();
            Mock<ISharedContentRedisInterface> redisMock;
            PagesController controller;
            SetupController(configuration, out redisMock, out controller);

            var result = await controller.Body(null, null, null) as NotFoundResult;

            // Assert
            result.StatusCode.Should().Be((int?)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PagesControllerBodyCalledWithValueReturnsCorrectView()
        {
            IConfiguration configuration = SetupConfiguration();
            Mock<ISharedContentRedisInterface> redisMock;
            PagesController controller;
            SetupController(configuration, out redisMock, out controller);

            // Act
            var result = await controller.Body("levelOne", "levelTwo", null);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;
           
            var model = viewResult.Model as TriageToolOptionViewModel;
            Assert.Equal("levelOne", model.SelectedLevelOne);
            Assert.Equal("levelTwo", model.SelectedLevelTwo);
        }

        [Theory]
        [MemberData(nameof(FilterTestData))]
        public async Task PagesControllerBodyReturnsArticlesAndProductsForSelectedLevelOneAndLevelTwo(string levelOne, string levelTwo)
        {
            IConfiguration configuration = SetupConfiguration();
            Mock<ISharedContentRedisInterface> redisMock;
            PagesController controller;
            SetupController(configuration, out redisMock, out controller);

            // Act
            var result = await controller.Body(levelOne, levelTwo, null);
            redisMock.VerifyAll();
            var viewResult = result as ViewResult;
            var model = viewResult.Model as TriageToolOptionViewModel;

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        private void SetupController(IConfiguration configuration, out Mock<ISharedContentRedisInterface> redisMock, out PagesController controller)
        {
            redisMock = new Mock<ISharedContentRedisInterface>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PagesController>>();

            redisMock.Setup(r => r.GetDataAsyncWithExpiry<TriageLookupResponse>("TriageTool/Lookup", "PUBLISHED", 4))
                     .ReturnsAsync(GetTriageLookupResponse());
            redisMock.Setup(r => r.GetDataAsyncWithExpiry<TriageResultPageResponse>("TriageTool/TriageResults", "PUBLISHED", 4))
                     .ReturnsAsync(GetTriagePageResponse());

            controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);
        }

        private static IConfiguration SetupConfiguration()
        {
            var contentMode = new Dictionary<string, string>
            {
                {"contentMode:contentMode", "PUBLISHED" },
            };
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(contentMode)
                .Build();
            return configuration;
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
                       FilterAdviceGroup = new FilterAdviceGroup
                       {
                           ContentItems = new List<FilterAdviceGroup>
                           {
                               new FilterAdviceGroup
                               {
                                   ContentItemId = "1",
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
                       TriageLevelTwo = new TriageLevelTwo
                       {
                           ContentItems = new List<TriageLevelTwo>
                           {
                               new TriageLevelTwo
                               {
                                   ContentItemId = "LevelTwo2",
                               },
                           },
                       },
                       FilterAdviceGroup = new FilterAdviceGroup
                       {
                           ContentItems = new List<FilterAdviceGroup>
                           {
                               new FilterAdviceGroup
                               {
                                   ContentItemId = "3",
                               },
                           },
                       },
                   },
                   new TriageResultPage
                   {
                       TriageOrdinal = 1,
                       DisplayText = "page 3",
                       TriageLevelOne = new TriageLevelOne
                       {
                           ContentItems = new List<TriageLevelOne>
                           {
                               new TriageLevelOne
                               {
                                   ContentItemId = "2",
                               },
                           },
                       },
                       TriageLevelTwo = new TriageLevelTwo
                       {
                           ContentItems = new List<TriageLevelTwo>
                           {
                               new TriageLevelTwo
                               {
                                   ContentItemId = "LevelTwo4",
                               },
                           },
                       },
                       FilterAdviceGroup = new FilterAdviceGroup
                       {
                           ContentItems = new List<FilterAdviceGroup>
                           {
                               new FilterAdviceGroup
                               {
                                   ContentItemId = "3",
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