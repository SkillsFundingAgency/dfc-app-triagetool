using AutoMapper;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
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
        public const string speakToanAdviserStaxId = "2c9da1b3-3529-4834-afc9-9cd741e59788";

        private readonly ILogger<PagesController> logger;
        private readonly AutoMapper.IMapper mapper;
        //private readonly IDocumentService<SharedContentItemModel> sharedContentItemDocumentService;
         private readonly IDocumentService<TriageToolOptionDocumentModel> triageToolDocumentService;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        public PagesController(
            ILogger<PagesController> logger,
            IMapper mapper,
            ISharedContentRedisInterface sharedContentRedis,
            IDocumentService<TriageToolOptionDocumentModel> triageToolDocumentService)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.sharedContentRedis = sharedContentRedis;
            this.triageToolDocumentService = triageToolDocumentService;
        }

        [HttpGet]
        [Route("pages/head")]
        [Route("pages/{triage-select?}/head")]
        public IActionResult Head([ModelBinder(Name = "triage-select")] string article)
        {
            logger.LogInformation($"{nameof(Head)} has been called");

            var viewModel = new HeadViewModel
            {
                Title = "Triage | National Careers Service",
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}", UriKind.RelativeOrAbsolute),
                Description = "Personalised careers advice and information",
            };

            logger.LogInformation($"{nameof(Head)} has returned content for: /{article}");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/breadcrumb")]
        [Route("pages/{triage-select?}/breadcrumb")]
        public IActionResult Breadcrumb([ModelBinder(Name = "triage-select")] string article)
        {
            logger.LogInformation($"{nameof(Breadcrumb)} has been called");

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
            logger.LogInformation($"{nameof(Breadcrumb)} has returned content ");

            return View(viewModel);
        }

        [HttpGet]
        [Route("pages/bodytop")]
        [Route("pages/{triage-select?}/bodytop")]
        public IActionResult BodyTop([ModelBinder(Name = "triage-select")] string article)
        {
            logger.LogWarning($"{nameof(BodyTop)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/body")]
        [Route("pages/{triage-select?}/body")]
        public async Task<IActionResult> Body([ModelBinder(Name = "triage-select")] string article)
        {
            //var documents = await triageToolDocumentService.GetAllAsync(TriageToolOptionDocumentModel.DefaultPartitionKey).ConfigureAwait(false);

            //var sortedDocuments = documents.OrderBy(o => o.Title).ToList();

            //var sharedContent = await sharedContentItemDocumentService.GetAllAsync().ConfigureAwait(false); need to change

            //var document = !string.IsNullOrWhiteSpace(article)
            //    ? sortedDocuments?.FirstOrDefault(x => string.Equals(x.Title, article, StringComparison.CurrentCultureIgnoreCase)) ?? sortedDocuments?.FirstOrDefault()
            //    : sortedDocuments?.FirstOrDefault();

            var triagetooldocuments = await sharedContentRedis.GetDataAsync<TriagePageResponse>("TriageToolPages");
     
            var pages = triagetooldocuments.Page;

            List<TriagePage> subList = new List<TriagePage>();

            foreach (var doc in pages)
            {
                foreach (var contentItem in doc.TriageToolFilters.ContentItems)
                {
                    if (contentItem.ToString().Contains(article)) 
                      subList.Add(doc);
                }
            }


            var triageToolModel = new TriageToolOptionViewModel
            {
                Title = article,
                Pages = subList,
            };

            //var model = mapper.Map<TriageToolOptionViewModel>(document);
            //model.SharedContent = sharedContent?.FirstOrDefault()?.Content;

            try
            {
                var sharedhtml = await sharedContentRedis.GetDataAsync<SharedHtml>("sharedContent/" + speakToanAdviserStaxId);
                triageToolModel.SharedContent = sharedhtml.Html;
            }
            catch
            {
                triageToolModel.SharedContent = "";
            }
            return View(triageToolModel);
        }

        [HttpGet]
        [Route("pages/bodyfooter")]
        [Route("pages/{triage-select?}/bodyfooter")]
        public IActionResult BodyFooter([ModelBinder(Name = "triage-select")] string article)
        {
            logger.LogWarning($"{nameof(BodyFooter)} has returned no content for: {article}");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/herobanner")]
        [Route("pages/{triage-select?}/herobanner")]
        public async Task<IActionResult> HeroBanner([ModelBinder(Name = "triage-select")] string article)
        {
            //var options = await triageToolDocumentService.GetAllAsync(TriageToolOptionDocumentModel.DefaultPartitionKey).ConfigureAwait(false);
            
            var triagetooldocuments = await sharedContentRedis.GetDataAsync<TriageToolFilterResponse>("TraigeToolFilters/All");
            var viewModel = new HeroBannerViewModel
            {
                Selected = article,
            };

            //if (options != null && options.Any())
            //{
            //    viewModel.Options = options.Select(x => x.Title).OrderBy(o => o).ToList()!;
            //}

            viewModel.Options = triagetooldocuments.TriageToolFilter.Select(x => x.DisplayText).ToList();

            return View(viewModel);
        }

        [HttpGet]
        [Route("api/TriageToolOption/GetOptions/ajax")]
        public async Task<IActionResult> Data()
        {
            //var documents = await triageToolDocumentService.GetAllAsync().ConfigureAwait(false);
            var triagetooldocuments = await sharedContentRedis.GetDataAsync<TriagePageResponse>("TriageToolPages");

            //var models = mapper.Map<IList<TriageToolOptionViewModel>>(documents.OrderBy(o => o.Title));
            var models1 = mapper.Map<IList<PageDocumentModel>>(triagetooldocuments.Page);
            return Json(models1);
        }
    }
}