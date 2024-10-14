using DFC.App.Triagetool.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NHibernate.Mapping;
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
            var result = controller.Breadcrumb("levelOne", "levelTwo");

            // Assert
            var statusResult = Assert.IsType<ViewResult>(result);

            var model = statusResult.Model as BreadcrumbViewModel;

            model!.Breadcrumbs.Count().Should().Be(2);
            model.Breadcrumbs.First().Route.Should().Be("/");
            model.Breadcrumbs.First().Title.Should().Be("Home");
            model.Breadcrumbs.Last().AddHyperlink.Should().Be(false);
            model.Breadcrumbs.Last().Title.Should().Be("Get relevant careers advice");
        }
    }
}
