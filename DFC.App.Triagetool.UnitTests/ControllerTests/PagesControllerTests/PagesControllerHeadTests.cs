using DFC.App.Triagetool.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NuGet.ContentModel;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [Trait("Category", "Pages Controller - Head Unit Tests")]
    public class PagesControllerHeadTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public void PagesControllerHeadHtmlReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.Head("levelOne", "levelTwo");

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(viewResult.ViewData.Model);

            model.CanonicalUrl.Should().NotBeNull();
            model.Description.Should().BeEquivalentTo("Get relevant careers advice");
            model.Title.Should().BeEquivalentTo("Get relevant careers advice");
        }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        public void PagesControllerHeadJsonReturnsSuccess(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = controller.Head("levelOne", "levelTwo");

            // Assert
            var jsonResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<HeadViewModel>(jsonResult.Value);

            model.CanonicalUrl.Should().NotBeNull();
            model.Description.Should().BeEquivalentTo("Get relevant careers advice");
            model.Title.Should().BeEquivalentTo("Get relevant careers advice");
        }
    }
}