using DFC.App.Triagetool.Controllers;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Compui.Cosmos.Contracts;
using FakeItEasy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DFC.App.Triagetool.UnitTests.ControllerTests.SitemapControllerTests
{
    public class BaseSitemapControllerTests
    {
        public BaseSitemapControllerTests()
        {
            FakeLogger = A.Fake<ILogger<SitemapController>>();
            FakeSharedContentRedisInterface = A.Fake<ISharedContentRedisInterface>();
        }

        protected ILogger<SitemapController> FakeLogger { get; }
        public ISharedContentRedisInterface FakeSharedContentRedisInterface { get; }
        protected IDocumentService<TriageToolOptionDocumentModel> FakeTriageToolDocumentService { get; }

        protected SitemapController BuildSitemapController()
        {
            var controller = new SitemapController(FakeLogger, FakeSharedContentRedisInterface)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext(),
                },
            };

            return controller;
        }
    }
}
