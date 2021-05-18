using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.Data.Models.ContentModels;
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
            new object[] { "/pages/body", "", nameof(PagesController.Body) },
            new object[] { "/pages/{article}/body", "", nameof(PagesController.Body) },
        };

        [Theory]
        [MemberData(nameof(PagesRouteDataOk))]
        public async Task PagesControllerCallsContentPageServiceUsingPagesRouteForOkResult(string route, string option, string actionMethod)
        {
            // Arrange
            using var controller = BuildController(route);
            var expectedResult = new SharedContentItemModel() { Content = "<h1>A document</h1>" };
            var expectedResults = new List<SharedContentItemModel> { expectedResult };

            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored)).Returns(expectedResults);
            A.CallTo(() => fakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored)).Returns(new List<TriageToolOptionDocumentModel>());

            // Act
            var result = await RunControllerAction(controller, option, actionMethod).ConfigureAwait(false);

            // Assert
            Assert.IsType<ViewResult>(result);
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceOrLess();
            A.CallTo(() => fakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored)).MustHaveHappenedOnceExactly();
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

            return new PagesController(Logger, FakeMapper, FakeSharedContentItemDocumentService, fakeTriageToolOptionDocumentService)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext,
                },
            };
        }
    }
}