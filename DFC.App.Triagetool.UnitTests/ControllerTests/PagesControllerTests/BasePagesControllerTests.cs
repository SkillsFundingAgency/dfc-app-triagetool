/*using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    public abstract class BasePagesControllerTests
    {
        protected BasePagesControllerTests()
        {
            Logger = A.Fake<ILogger<PagesController>>();
            FakeSharedContentItemDocumentService = A.Fake<IDocumentService<SharedContentItemModel>>();
            FakeTriageToolOptionDocumentService = A.Fake<IDocumentService<TriageToolOptionDocumentModel>>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
        }

        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new string[] { "*//**" },
            new string[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> InvalidMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Text.Plain },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Application.Json },
        };

        protected ILogger<PagesController> Logger { get; }

        protected IDocumentService<SharedContentItemModel> FakeSharedContentItemDocumentService { get; }

        protected IDocumentService<TriageToolOptionDocumentModel> FakeTriageToolOptionDocumentService { get; }

        protected ISharedContentRedisInterface SharedContentRedisInterface { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected PagesController BuildPagesController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new PagesController(Logger, FakeMapper, SharedContentRedisInterface, FakeSharedContentItemDocumentService)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }

        protected List<TriageToolOptionDocumentModel> Getdocuments()
        {
            return new List<TriageToolOptionDocumentModel>()
            {
                new TriageToolOptionDocumentModel
                {
                    Filters = new List<TriageToolFilterDocumentModel>
                    {
                        new TriageToolFilterDocumentModel
                        {
                            Title = "test",
                            Url = new Uri("https://Uri1.com/"),
                        },
                    },
                    Title = "page 1",
                },
                new TriageToolOptionDocumentModel
                {
                    Filters = new List<TriageToolFilterDocumentModel>
                    {
                        new TriageToolFilterDocumentModel
                        {
                            Title = "test",
                            Url = new Uri("https://Uri1.com"),
                        },
                        new TriageToolFilterDocumentModel
                        {
                            Title = "test 2",
                            Url = new Uri("https://Uri2.com"),
                        },
                    },
                    Pages = new List<PageDocumentModel>
                    {
                        new PageDocumentModel
                        {
                            Filters = new List<string>
                            {
                                "https://Uri2.com",
                            },
                            Uri = new Uri("https://Page1.com"),
                        },
                        new PageDocumentModel
                        {
                            Filters = new List<string>
                            {
                                "https://Uri1.com",
                            },
                            Uri = new Uri("https://Page2.com"),
                        },
                    },
                    Title = "option 2",
                },
            };
        }
    }
}
*/