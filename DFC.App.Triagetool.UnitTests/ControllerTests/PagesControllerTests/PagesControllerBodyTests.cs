using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.ViewModels;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    [Trait("Category", "Pages Controller - Body Unit Tests")]
    public class PagesControllerBodyTests : BasePagesControllerTests
    {
        [Theory]
        [MemberData(nameof(HtmlMediaTypes))]
        public async Task PagesControllerBodyReturnsViewWhenNoDataFound(string mediaTypeName)
        {
            // Arrange
            using var controller = BuildPagesController(mediaTypeName);
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(new List<TriageToolOptionDocumentModel>());
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored)).Returns(new List<SharedContentItemModel>());

            // Act
            var result = await controller.Body("an-article").ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            var statusResult = Assert.IsType<ViewResult>(result);

            var model = statusResult.Model as TriageToolOptionViewModel;

            Assert.Empty(model!.Pages);
        }

        [Fact]
        public async Task PagesControllerBodyCalledWithValueReturnsCorrectView()
        {
            var pageTitle = "page 1";

            // Arrange
            using var controller = BuildPagesController(nameof(HtmlMediaTypes));

            var documents = new List<TriageToolOptionDocumentModel>()
            {
                new TriageToolOptionDocumentModel
                {
                    Filters = new List<TriageToolFilterDocumentModel>
                    {
                        new TriageToolFilterDocumentModel
                        {
                            Title = "test",
                            Url = new Uri("https://Uri1.com"),
                        },
                    },
                    Title = pageTitle,
                },
            };

            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(documents);
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored)).Returns(new List<SharedContentItemModel>());

            // Act
            var result = await controller.Body(pageTitle).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() =>
                FakeMapper.Map<TriageToolOptionViewModel>(
                    A<TriageToolOptionDocumentModel>.That.Matches(x => x.Title == pageTitle))).MustHaveHappened();
            var statusResult = Assert.IsType<ViewResult>(result);

            var model = statusResult.Model as TriageToolOptionViewModel;

            Assert.Empty(model!.Pages);
        }

        [Fact]
        public async Task PagesControllerBodyReturnsFirstDocumentWhenCalledWithOutOption()
        {
            var documents = Getdocuments();
            var pageTitle = documents.Select(s => s.Title).OrderBy(o => o).First();

            // Arrange
            using var controller = BuildPagesController(nameof(HtmlMediaTypes));

            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(documents);
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored)).Returns(new List<SharedContentItemModel>());

            // Act
            var result = await controller.Body(string.Empty).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() =>
                FakeMapper.Map<TriageToolOptionViewModel>(
                    A<TriageToolOptionDocumentModel>.That.Matches(x => x.Title == pageTitle))).MustHaveHappened();
            var statusResult = Assert.IsType<ViewResult>(result);

            var model = statusResult.Model as TriageToolOptionViewModel;

            Assert.Empty(model!.Pages);
        }

        [Fact]
        public async Task PagesControllerPostBodyReturnsCorrectPagesWhenCalledWithFilters()
        {
            var pageTitle = "option 2";

            // Arrange
            using var controller = BuildPagesController(nameof(HtmlMediaTypes));

            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(Getdocuments());
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored)).Returns(new List<SharedContentItemModel>());
            A.CallTo(() => FakeMapper.Map<TriageToolOptionViewModel>(A<TriageToolOptionDocumentModel>.Ignored)).Returns(
                new TriageToolOptionViewModel()
                {
                    Filters = new List<TriageToolFilterViewModel>
                    {
                        new TriageToolFilterViewModel
                        {
                            Title = "test",
                            Url = new Uri("https://Uri1.com/"),
                        },
                    },
                });

            // Act
            var result = await controller.Post(new OptionPostViewModel
            {
                Filters = "https://Uri1.com/",
                Title = pageTitle,
            }).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() =>
                FakeMapper.Map<TriageToolOptionViewModel>(
                    A<TriageToolOptionDocumentModel>.That.Matches(x => x.Pages.Any(p => p.Uri == new Uri("https://Page2.com"))))).MustNotHaveHappened();
            var statusResult = Assert.IsType<ViewResult>(result);

            var model = statusResult.Model as TriageToolOptionViewModel;

            Assert.True(model!.SelectedFilters.Any());
            Assert.Empty(model.Pages);
        }

        [Fact]
        public async Task PagesControllerPostBodyReturnsViewWhenNoDataFound()
        {
            // Arrange
            using var controller = BuildPagesController(nameof(HtmlMediaTypes));
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(new List<TriageToolOptionDocumentModel>());
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored)).Returns(new List<SharedContentItemModel>());

            // Act
            await Assert.ThrowsAsync<FileNotFoundException>(() => controller.Post(new OptionPostViewModel())).ConfigureAwait(false);

            // Assert
            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .MustHaveHappenedOnceExactly();
            A.CallTo(() => FakeSharedContentItemDocumentService.GetAllAsync(A<string>.Ignored))
                .MustNotHaveHappened();
        }

        [Fact]
        public async Task PagesControllerDataReturnsJson()
        {
            using var controller = BuildPagesController(nameof(HtmlMediaTypes));

            A.CallTo(() => FakeTriageToolOptionDocumentService.GetAllAsync(A<string>.Ignored))
                .Returns(Getdocuments());

            var result = await controller.Data().ConfigureAwait(false);

            Assert.IsType<JsonResult>(result);
        }
    }
}
