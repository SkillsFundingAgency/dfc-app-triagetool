using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DFC.App.Triagetool.Controllers;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using Microsoft.Extensions.Logging;
using AutoMapper;


namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller - HeroBanner Unit Tests")]
    public class PagesControllerHeroBannerTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeroBannerReturnsNoContentWhenNoData(string mediaTypeName)
        {
            var redisMock = new Mock<ISharedContentRedisInterface>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PagesController>>();
            var optionsMock = new Mock<TriageTooltOptionsDocumentModel>();
           

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object);

            // Act
            var result = await controller.HeroBanner(null); // Pass null to simulate no data

            // Assert
            Assert.IsType<NoContentResult>(result);

        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeroBannerReturnsViewtWhenOptionsFound(string mediaTypeName)
        {
            var redisMock = new Mock<ISharedContentRedisInterface>();
            var mapperMock = new Mock<IMapper>();
            var loggerMock = new Mock<ILogger<PagesController>>();
            var optionsMock = new Mock<TriageTooltOptionsDocumentModel>();
            var triageToolFilterResponse = new TriageToolFilterResponse
            {
                TriageToolFilter = new List<TriageToolFilters>
            {
                new () { DisplayText = "Option1" },
                new () { DisplayText = "Option2" },
            },
            };
            redisMock.Setup(r => r.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All"))
                     .ReturnsAsync(triageToolFilterResponse);

            var controller = new PagesController(loggerMock.Object, mapperMock.Object,redisMock.Object);

            // Act
            var result = await controller.HeroBanner("article") as ViewResult;
            var viewModel = result?.Model as HeroBannerViewModel;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(viewModel);
            Assert.Equal("article", viewModel.Selected);
            Assert.Equal(2, viewModel.Options.Count);
            Assert.Equal("Option1", viewModel.Options[0]);
            Assert.Equal("Option2", viewModel.Options[1]);
        }
    }
 }

