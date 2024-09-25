using AutoMapper;
using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using AppConstants = DFC.Common.SharedContent.Pkg.Netcore.Constant.ApplicationKeys;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller - HeroBanner Unit Tests")]
    public class PagesControllerHeroBannerTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeroBannerReturnsSuccessWithSelectedLevelOneAndLevelTwo(string mediaTypeName)
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

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);

            // Act
            var result = controller.HeroBanner("levelOne", "levelTwo"); // Pass null to simulate no data

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HeroBannerViewModel>(viewResult.ViewData.Model);
            model.SelectedLevelOne.Should().Be("levelOne");
            model.SelectedLevelTwo.Should().Be("levelTwo");
        }
    }
}