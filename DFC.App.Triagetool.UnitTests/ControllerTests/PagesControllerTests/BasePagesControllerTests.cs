using DFC.App.Triagetool.Controllers;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using FakeItEasy;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.PagesControllerTests
{
    [ExcludeFromCodeCoverage]
    public abstract class BasePagesControllerTests
    {
        protected BasePagesControllerTests()
        {
            Logger = A.Fake<ILogger<PagesController>>();
            FakeSharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();
            FakeMapper = A.Fake<AutoMapper.IMapper>();
            FakeConfiguration = A.Fake<IConfiguration>();
        }
        public static IEnumerable<object[]> HtmlMediaTypes => new List<object[]>
        {
            new string[] { "*/*" },
            new string[] { MediaTypeNames.Text.Html },
        };

        public static IEnumerable<object[]> JsonMediaTypes => new List<object[]>
        {
            new string[] { MediaTypeNames.Application.Json },
        };

        protected ILogger<PagesController> Logger { get; }

        public ISharedContentRedisInterface FakeSharedContentRedisInterface { get; }

        public IConfiguration FakeConfiguration { get; }

        protected AutoMapper.IMapper FakeMapper { get; }

        protected PagesController BuildPagesController(string mediaTypeName)
        {
            var httpContext = new DefaultHttpContext();

            httpContext.Request.Headers[HeaderNames.Accept] = mediaTypeName;

            var controller = new PagesController(Logger, FakeMapper, FakeSharedContentRedisInterface, FakeConfiguration)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                },
            };

            return controller;
        }
    }
}
