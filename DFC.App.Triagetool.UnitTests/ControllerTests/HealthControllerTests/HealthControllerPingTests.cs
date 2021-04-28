using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.HealthControllerTests
{
    [Trait("Category", "Health Controller Unit Tests")]
    public class HealthControllerPingTests : BaseHealthControllerTests
    {
        [Fact]
        public void HealthControllerPingReturnsSuccess()
        {
            // Arrange
            using var controller = BuildHealthController(MediaTypeNames.Application.Json);

            // Act
            var result = controller.Ping();

            // Assert
            var statusResult = Assert.IsType<OkResult>(result);

            A.Equals((int)HttpStatusCode.OK, statusResult.StatusCode);
        }
    }
}
