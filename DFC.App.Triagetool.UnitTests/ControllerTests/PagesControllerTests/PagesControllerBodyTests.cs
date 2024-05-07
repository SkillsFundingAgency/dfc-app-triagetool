using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Moq;
using DFC.App.Triagetool.Controllers;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.Common.SharedContent.Pkg.Netcore;
using AutoMapper;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using Microsoft.Extensions.Logging;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.CodeAnalysis.FlowAnalysis;

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

            redisMock.Setup(r => r.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All", "PUBLISHED"))
                     .ReturnsAsync((TriageToolFilterResponse) null);

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);

            // Act
            var result = await controller.Body("YourArticleId") as ViewResult;

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
            redisMock.Setup(r => r.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All", "PUBLISHED"))
                     .ReturnsAsync(triageToolFilterResponse);

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);

            var mockSharedContentRedis = new Mock<ISharedContentRedisInterface>();
            var triagePageResponse = new TriagePageResponse();
            mockSharedContentRedis.Setup(x => x.GetDataAsync<TriagePageResponse>("TriageToolPages", "PUBLISHED")).ReturnsAsync(triagePageResponse);

            // Act
            var result = await controller.Body("Article");

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
            redisMock.Setup(r => r.GetDataAsync<TriageToolFilterResponse>("TriageToolFilters/All", "PUBLISHED"))
                     .ReturnsAsync(triageToolFilterResponse);

            var controller = new PagesController(loggerMock.Object, mapperMock.Object, redisMock.Object, configuration);

            // Mocking sharedContentRedis.GetDataAsync<TriagePageResponse>
            var mockSharedContentRedis = new Mock<ISharedContentRedisInterface>();
            var triagePageResponse = new TriagePageResponse();
            mockSharedContentRedis.Setup(x => x.GetDataAsync<TriagePageResponse>("TriageToolPages", "PUBLISHED")).ReturnsAsync(triagePageResponse);

            // Act
            var result = await controller.Body("Article");

            // Assert
            Assert.IsType<ViewResult>(result);
        }

    }
}