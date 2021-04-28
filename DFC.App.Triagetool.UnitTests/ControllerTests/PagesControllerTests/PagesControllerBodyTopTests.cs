using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - BodyTop Unit Tests")]
    public class PagesControllerBodyTopTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBodyTopReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.BodyTop("an-article").ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);
        }
    }
}
