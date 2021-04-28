using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - Body Unit Tests")]
    public class PagesControllerBodyTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBodyReturnsNotFoundWhenNoData(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.Body("an-article").ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<NotFoundResult>(result);

            A.Equals((int)HttpStatusCode.NotFound, statusResult.StatusCode);
        }
    }
}
