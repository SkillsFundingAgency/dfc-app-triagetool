using DFC.App.Triagetool.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller - Breadcrumb Unit Tests")]
    public class PagesControllerBreadcrumbTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerBreadcrumbReturnsBreadCrumb(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.Breadcrumb("an-article");

            // Assert
            var statusResult = Assert.IsType<ViewResult>(result);

            var model = statusResult.Model as BreadcrumbViewModel;

            Assert.True(model!.Breadcrumbs.Any());
        }
    }
}
