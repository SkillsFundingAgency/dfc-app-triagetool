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
using System.Linq;
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
            new object[] { "Education", "LevelTwo1", null, null, null,
                new List<FilterAdviceGroup> { GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse()?.FilterAdviceGroup[1], },
                new List<FilterAdviceGroup>{ GetTriageLookupResponse()?.FilterAdviceGroup[0], GetTriageLookupResponse()?.FilterAdviceGroup[1], },
                new List<TriageResultPage> { GetTriagePageResponse()?.Page[0], },
                },
            new object[] { null, null,"Education|LevelTwo1",null,null,
                new List<FilterAdviceGroup> { GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[1], },
                new List<FilterAdviceGroup>{ GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[1], },
                new List<TriageResultPage> { GetTriagePageResponse().Page[0], },
                },
            new object[] { "Education", "LevelTwo1",null,new List<string> {GetTriageLookupResponse().FilterAdviceGroup[0].Title },TriageToolFilerAction.ClearFilters,
                new List<FilterAdviceGroup> { GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[1], },
                new List<FilterAdviceGroup>{ GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[1], },
                new List<TriageResultPage> { GetTriagePageResponse().Page[0], },
                },
            new object[] { "Education", "LevelTwo1",null,new List<string> {GetTriageLookupResponse().FilterAdviceGroup[0].ContentItemId },TriageToolFilerAction.ApplyFilters,
                new List<FilterAdviceGroup> { GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[1], },
                new List<FilterAdviceGroup>{ GetTriageLookupResponse().FilterAdviceGroup[0] },
                new List<TriageResultPage> { GetTriagePageResponse().Page[0], },
                },
            new object[] { "Education", "LevelTwo4",null,null,null,
                null,
                null,
                new List<TriageResultPage>(),
                },
            new object[] { "Education", "LevelTwo2",null,null,null,
                new List<FilterAdviceGroup> { GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[2], },
                new List<FilterAdviceGroup>{ GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[2], },
                new List<TriageResultPage> { GetTriagePageResponse().Page[0], GetTriagePageResponse().Page[1], },
                },
            new object[] { "Employed", "LevelTwo4",null,null,null,
                new List<FilterAdviceGroup> { GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[2], },
                new List<FilterAdviceGroup>{ GetTriageLookupResponse().FilterAdviceGroup[0], GetTriageLookupResponse().FilterAdviceGroup[2], },
                new List<TriageResultPage> { GetTriagePageResponse().Page[2], GetTriagePageResponse().Page[3],GetTriagePageResponse().Page[4] },
                },
            new object[] { "Not In Work", "LevelTwo7",null,null,null,
                new List<FilterAdviceGroup> { GetTriageLookupResponse().FilterAdviceGroup[2], GetTriageLookupResponse().FilterAdviceGroup[3], },
                new List<FilterAdviceGroup>{ GetTriageLookupResponse().FilterAdviceGroup[2], GetTriageLookupResponse().FilterAdviceGroup[3], },
                new List<TriageResultPage> { GetTriagePageResponse().Page[4], GetTriagePageResponse().Page[3] },
                },
        };

        public static IEnumerable<object[]> SelectedLevels => new List<object[]>
        {
            new object[] { string.Empty, string.Empty, string.Empty },
            new object[] { null, null, null },
            new object[] { "levelOne", string.Empty, string.Empty },
            new object[] { string.Empty, "levelTwo", string.Empty },
            new object[] { string.Empty, string.Empty, "singleselect" },
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
            var result = await controller.Body("YourArticleId", "Article", null, null, null) as ViewResult;

            // Assert
            Assert.Equal(null, result.ContentType);
            redisMock.VerifyAll();
        }

        [Theory]
        [MemberData(nameof(SelectedLevels))]
        public async Task PagesControllerBodyCalledWithNullFactorValuesShoutReturnError(string levelOne, string levelTwo, string singleSelect)
        {
            IConfiguration configuration = SetupConfiguration();
            Mock<ISharedContentRedisInterface> redisMock;
            PagesController controller;
            SetupController(configuration, out redisMock, out controller);

            var result = await controller.Body(levelOne, levelTwo, singleSelect, null,null) as ViewResult;

            // Assert
            result.ViewName.Should().Be("Error");
        }

        [Fact]
        public async Task PagesControllerBodyCalledWithValueReturnsCorrectView()
        {
            IConfiguration configuration = SetupConfiguration();
            Mock<ISharedContentRedisInterface> redisMock;
            PagesController controller;
            SetupController(configuration, out redisMock, out controller);

            // Act
            var result = await controller.Body("levelOne", "levelTwo", null, null, null);

            // Assert
            Assert.IsType<ViewResult>(result);
            var viewResult = result as ViewResult;

            var model = viewResult.Model as TriageToolOptionViewModel;
            Assert.Equal("levelOne", model.SelectedLevelOne);
            Assert.Equal("levelTwo", model.SelectedLevelTwo);
        }

        [Theory]
        [MemberData(nameof(FilterTestData))]
        public async Task PagesControllerBodyReturnsArticlesAndProductsForSelectedLevelOneAndLevelTwo(string levelOne, string levelTwo, string singleSelect,
                                                                                                       List<string>? selectedFilters,
                                                                                                       TriageToolFilerAction? action,
                                                                                                       List<FilterAdviceGroup> expectedAllAdviceGroups,
                                                                                                       List<FilterAdviceGroup> expectedSelectedFilterAdviceGroup,
                                                                                                       List<TriageResultPage> expectedResults)
        {
            IConfiguration configuration = SetupConfiguration();
            Mock<ISharedContentRedisInterface> redisMock;
            PagesController controller;
            SetupController(configuration, out redisMock, out controller);

            // Act
            var result = await controller.Body(levelOne, levelTwo, singleSelect, selectedFilters, action);
            redisMock.VerifyAll();
            var viewResult = result as ViewResult;
            var model = viewResult.Model as TriageToolOptionViewModel;

            // Assert
            Assert.IsType<ViewResult>(result);
            model.Pages.Should().BeEquivalentTo(expectedResults);
            model.AllFilterAdviceGroups.Should().BeEquivalentTo(expectedAllAdviceGroups);
            model.FilterAdviceGroups.Should().BeEquivalentTo(expectedSelectedFilterAdviceGroup);
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
                    Value = "Not In Work",
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

        private static TriageResultPageResponse? GetTriagePageResponse()
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
                   new TriageResultPage
                   {
                       TriageOrdinal = 4,
                       DisplayText = "page 4",
                       TriageLevelOne = new TriageLevelOne
                       {
                           ContentItems = new List<TriageLevelOne>
                           {
                               new TriageLevelOne
                               {
                                   ContentItemId = "2",
                               },
                               new TriageLevelOne
                               {
                                   ContentItemId = "3",
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
                               new TriageLevelTwo
                               {
                                   ContentItemId = "LevelTwo7",
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
                       TriageOrdinal = 5,
                       DisplayText = "page 5",
                       TriageLevelOne = new TriageLevelOne
                       {
                           ContentItems = new List<TriageLevelOne>
                           {
                               new TriageLevelOne
                               {
                                   ContentItemId = "2",
                               },
                               new TriageLevelOne
                               {
                                   ContentItemId = "3",
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
                               new TriageLevelTwo
                               {
                                   ContentItemId = "LevelTwo7",
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