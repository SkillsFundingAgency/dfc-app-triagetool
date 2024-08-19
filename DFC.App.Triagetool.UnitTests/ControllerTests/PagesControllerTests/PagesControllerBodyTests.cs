using AutoMapper;
using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
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

    }
}