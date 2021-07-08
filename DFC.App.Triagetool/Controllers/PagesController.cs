using AutoMapper;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.ViewModels;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IDocumentService<TriageToolOptionDocumentModel> triageToolDocumentService;

        public PagesController(
            ILogger<PagesController> logger,
            IMapper mapper,
            IDocumentService<SharedContentItemModel> sharedContentItemDocumentService,
            IDocumentService<TriageToolOptionDocumentModel> triageToolDocumentService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.sharedContentItemDocumentService = sharedContentItemDocumentService;
            this.triageToolDocumentService = triageToolDocumentService;
        }

        [HttpGet]
        [Route("pages/htmlhead")]
        [Route("pages/{article?}/htmlhead")]
        public async Task<IActionResult> HtmlHead(string article)
        {
            logger.LogWarning($"{nameof(HtmlHead)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/breadcrumb")]
        [Route("pages/{article?}/breadcrumb")]
        public async Task<IActionResult> Breadcrumb(string article)
        {
            const string slash = "/";
            var viewModel = new BreadcrumbViewModel
            {
                Breadcrumbs = new List<BreadcrumbItemViewModel>()
                {
                    new BreadcrumbItemViewModel()
                    {
                        Route = slash,
                        Title = "Home",
                    },
                    new BreadcrumbItemViewModel
                    {
                        AddHyperlink = false,
                        Title = "Personalised careers advice and information",
                    },
                },
            };
            logger.LogWarning($"{nameof(Breadcrumb)} has returned no content for: {article}");

            return View(viewModel);
        }

        [HttpGet]
        [Route("pages/bodytop")]
        [Route("pages/{article?}/bodytop")]
        public async Task<IActionResult> BodyTop(string article)
        {
            logger.LogWarning($"{nameof(BodyTop)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/body")]
        [Route("pages/{article?}/body")]
        public async Task<IActionResult> Body(string article)
        {
            var documents = await triageToolDocumentService
                .GetAllAsync(TriageToolOptionDocumentModel.DefaultPartitionKey).ConfigureAwait(false);
            var sortedDocuments = documents.OrderBy(o => o.Title).ToList();
            var sharedContent = await sharedContentItemDocumentService.GetAllAsync().ConfigureAwait(false);
            var document = !string.IsNullOrWhiteSpace(article)
                ? sortedDocuments?.FirstOrDefault(x => string.Equals(x.Title, article, StringComparison.CurrentCultureIgnoreCase)) ?? sortedDocuments?.FirstOrDefault()
                : sortedDocuments?.FirstOrDefault();

            var model = mapper.Map<TriageToolOptionViewModel>(document);
            model.SharedContent = sharedContent?.FirstOrDefault()?.Content;

            return View(model);
        }

        [HttpPost]
        [Route("pages/body")]
        [Route("pages/{article}/body")]
        public async Task<IActionResult> Post(OptionPostViewModel postData)
        {
            var documents = (await triageToolDocumentService
                .GetAllAsync(TriageToolOptionDocumentModel.DefaultPartitionKey).ConfigureAwait(false))?.ToList();

            if (documents == null || !documents.Any())
            {
                throw new FileNotFoundException("Unable to Find any Triage Tool documents");
            }

            var sortedDocuments = documents.OrderBy(o => o.Title).ToList();
            var document = sortedDocuments?.FirstOrDefault(x => string.Equals(x.Title, postData.Title, StringComparison.CurrentCultureIgnoreCase)) ?? sortedDocuments?.FirstOrDefault();

            if (postData?.Filters != null && postData.Filters.Split(",").Any() && document != null)
            {
                document.Pages = document.Pages.Where(p => postData.Filters.Split(",").Any(pdf => p.Filters.Contains(pdf))).ToList();
            }

            var model = mapper.Map<TriageToolOptionViewModel>(document);

            if (postData?.Filters != null && postData.Filters.Any())
            {
                foreach (var filter in model.Filters)
                {
                    filter.Selected = postData.Filters.Split(",").Any(pdf => string.Equals(pdf, filter?.Url?.ToString(), StringComparison.CurrentCultureIgnoreCase));
                }

                model.SelectedFilters = postData.Filters.Split(",").ToList();
            }

            var sharedContent = await sharedContentItemDocumentService.GetAllAsync().ConfigureAwait(false);
            model.SharedContent = sharedContent?.FirstOrDefault()?.Content;

            return View("Body", model);
        }

        [HttpGet]
        [Route("pages/bodyfooter")]
        [Route("pages/{article?}/bodyfooter")]
        public async Task<IActionResult> BodyFooter(string article)
        {
            logger.LogWarning($"{nameof(BodyFooter)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/herobanner")]
        [Route("pages/{article?}/herobanner")]
        public async Task<IActionResult> HeroBanner(string article)
        {
            var options = await triageToolDocumentService.GetAllAsync(TriageToolOptionDocumentModel.DefaultPartitionKey).ConfigureAwait(false);

            var viewModel = new HeroBannerViewModel();

            if (options != null && options.Any())
            {
                viewModel.Options = options.Select(x => x.Title).OrderBy(o => o).ToList()!;
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("api/TriageToolOption/GetOptions/ajax")]
        public async Task<IActionResult> Data()
        {
            var documents = await triageToolDocumentService.GetAllAsync().ConfigureAwait(false);
            var models = mapper.Map<IList<TriageToolOptionViewModel>>(documents.OrderBy(o => o.Title));
            return Json(models);
        }
    }
}