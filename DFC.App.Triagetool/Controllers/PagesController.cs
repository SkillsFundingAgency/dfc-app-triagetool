using AutoMapper;
using DFC.App.Triagetool.Data.Models;
using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.Models;
using DFC.App.Triagetool.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Controllers
{
    public class PagesController : Controller
    {
        public const string BradcrumbTitle = "Triage tool";
        public const string RegistrationPath = "triagetool";
        public const string LocalPath = "pages";
        public const string DefaultPageTitleSuffix = BradcrumbTitle + " | National Careers Service";
        public const string PageTitleSuffix = " | " + DefaultPageTitleSuffix;

        private readonly ILogger<PagesController> logger;
        private readonly AutoMapper.IMapper mapper;
        private readonly IDocumentService<SharedContentItemModel> sharedContentItemDocumentService;

        public PagesController(
            ILogger<PagesController> logger,
            IMapper mapper,
            IDocumentService<SharedContentItemModel> sharedContentItemDocumentService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.sharedContentItemDocumentService = sharedContentItemDocumentService;
        }

        [HttpGet]
        [Route("/")]
        [Route("pages")]
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel()
            {
                LocalPath = LocalPath,
                Documents = new List<IndexDocumentViewModel>()
                {
                    new IndexDocumentViewModel { Title = HealthController.HealthViewCanonicalName },
                    new IndexDocumentViewModel { Title = SitemapController.SitemapViewCanonicalName },
                    new IndexDocumentViewModel { Title = RobotController.RobotsViewCanonicalName },
                },
            };
            var sharedContentItemModels = await sharedContentItemDocumentService.GetAllAsync().ConfigureAwait(false);

            if (sharedContentItemModels != null)
            {
                var documents = from a in sharedContentItemModels.OrderBy(o => o.Title)
                                select mapper.Map<IndexDocumentViewModel>(a);

                viewModel.Documents.AddRange(documents);

                logger.LogInformation($"{nameof(Index)} has succeeded");
            }
            else
            {
                logger.LogWarning($"{nameof(Index)} has returned with no results");
            }

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/{documentId}/document")]
        public async Task<IActionResult> Document(Guid documentId)
        {
            var sharedContentItemModel = await sharedContentItemDocumentService.GetByIdAsync(documentId).ConfigureAwait(false);

            if (sharedContentItemModel != null)
            {
                var viewModel = mapper.Map<DocumentViewModel>(sharedContentItemModel);
                var breadcrumbItemModel = mapper.Map<BreadcrumbItemModel>(sharedContentItemModel);

                viewModel.Breadcrumb = BuildBreadcrumb(LocalPath, breadcrumbItemModel);

                logger.LogInformation($"{nameof(Document)} has succeeded for: {documentId}");

                return this.NegotiateContentResult(viewModel);
            }

            logger.LogWarning($"{nameof(Document)} has returned no content for: {documentId}");

            return NoContent();
        }

        private static BreadcrumbViewModel BuildBreadcrumb(string segmentPath, BreadcrumbItemModel? breadcrumbItemModel)
        {
            var viewModel = new BreadcrumbViewModel
            {
                Breadcrumbs = new List<BreadcrumbItemViewModel>()
                {
                    new BreadcrumbItemViewModel()
                    {
                        Route = "/",
                        Title = "Home",
                    },
                },
            };

            if (breadcrumbItemModel?.Title != null &&
                !string.IsNullOrWhiteSpace(breadcrumbItemModel.Route))
            {
                var articlePathViewModel = new BreadcrumbItemViewModel
                {
                    Route = "/" + segmentPath,
                    Title = breadcrumbItemModel.Title,
                };

                viewModel.Breadcrumbs.Add(articlePathViewModel);
            }

            viewModel.Breadcrumbs.Last().AddHyperlink = false;

            return viewModel;
        }
    }
}