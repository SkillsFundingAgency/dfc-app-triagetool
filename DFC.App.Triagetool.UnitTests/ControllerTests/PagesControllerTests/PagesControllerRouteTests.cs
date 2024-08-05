using DFC.App.Triagetool.Controllers;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller Unit Tests")]
    public class PagesControllerRouteTests : BasePagesControllerTests
    {
        public static IEnumerable<object[]> PagesRouteDataOk => new List<object[]>
        {
            new object[] { "/pages/body", string.Empty, nameof(PagesController.Body) },
            new object[] { "/pages/{article}/body", string.Empty, nameof(PagesController.Body) },
        };

        [Theory]
        [MemberData(nameof(PagesRouteDataOk))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesRouteForOkResult(string route, string option, string actionMethod)
        {
            // Arrange
            using var controller = BuildController(route);
            option = "Test";
            var expected = new TriageToolFilterResponse()
            {
                TriageToolFilter = new List<TriageToolFilters>
                {
                    new ()
                    {
                        DisplayText = "Test",
                    },
                },
            };

            A.CallTo(() => FakeSharedContentRedisInterface.GetDataAsync<TriageToolFilterResponse>("TriageTool/TriageToolFilters/All", "PUBLISHED", 4)).Returns(expected);

            // Act
            var result = await RunControllerAction(controller, option, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<ViewResult>(result);
            controller.Dispose();
        }

        private static async Task<IActionResult> RunControllerAction(PagesController controller, string option, string actionName)
        {
            return actionName switch
            {
                _ => await controller.Body(option).ConfigureAwait(false),
            };
        }

        private PagesController BuildController(string route)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = route;
            httpContext.Request.Headers[HeaderNames.Accept] = MediaTypeNames.Application.Json;

            return new PagesController(Logger, FakeMapper, FakeSharedContentRedisInterface, FakeConfiguration)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}