using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - HeroBanner Unit Tests")]
    public class PagesControllerHeroBannerTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeroBannerReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.HeroBanner("an-article").ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<NoContentResult>(result);

            A.Equals((int)HttpStatusCode.NoContent, statusResult.StatusCode);
        }
    }
}
