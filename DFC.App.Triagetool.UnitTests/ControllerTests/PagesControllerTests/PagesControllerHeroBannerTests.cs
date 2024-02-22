using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller - HeroBanner Unit Tests")]
    public class PagesControllerHeroBannerTests : BasePagesControllerTests
    {
        /*[Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeroBannerReturnsNoContentWhenNoData(string mediaTypeName)
        {
            // Arrange
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(new List<TriageToolOptionDocumentModel>());
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.HeroBanner("an-article").ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<ViewResult>(result);
            }

        [Theory]
        [MemberData(nameof(JsonMediaTypes))]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerHeroBannerReturnsViewtWhenOptionsFound(string mediaTypeName)
        {
            // Arrange
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(Getdocuments());
            using var controller = BuildPagesController(mediaTypeName);

            // Act
            var result = await controller.HeroBanner("an-article").ConfigureAwait(false);

            // Assert
            var statusResult = Assert.IsType<ViewResult>(result);
            Assert.True(((HeroBannerViewModel)statusResult.Model).Options.Any());
        }
    }*/
    }
}
