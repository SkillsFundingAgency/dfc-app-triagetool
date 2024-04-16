using AutoMapper;
using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.Models;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Constants = DFC.Common.SharedContent.Pkg.Netcore.Constant.ApplicationKeys;


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
        private readonly ISharedContentRedisInterface sharedContentRedis;

        public PagesController(
            ILogger<PagesController> logger,
            IMapper mapper,
            ISharedContentRedisInterface sharedContentRedis)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.sharedContentRedis = sharedContentRedis;
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
            var triagetooldocuments = await sharedContentRedis.GetDataAsync<TriagePageResponse>(Constants.TriagePages, "PUBLISHED");
            if (article != null)
            {
                article = string.Concat(article[0].ToString().ToUpper(), article.AsSpan(1));
            }

            var subList = triagetooldocuments?.Page?.Where(doc => doc.TriageToolFilters.ContentItems.Any(tp => tp.DisplayText == article)).ToList();

            var triageToolModel = new TriageToolOptionViewModel
            {
                Title = article,
                Pages = subList,
            };

            try
            {
                var sharedhtml = await this.sharedContentRedis.GetDataAsync<SharedHtml>(Constants.SpeakToAnAdviserSharedContent, "PUBLISHED");
                triageToolModel.SharedContent = sharedhtml?.Html;
            }
            catch
            {
                triageToolModel.SharedContent = string.Empty;
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
            var triageFilters = await sharedContentRedis.GetDataAsync<TriageToolFilterResponse>(Constants.TriageToolFilters, "PUBLISHED");

            var viewModel = new HeroBannerViewModel
            {
                Selected = article,
            };

            if (triageFilters != null)
            {
                viewModel.Options = triageFilters?.TriageToolFilter.Select(x => x.DisplayText).OrderBy(o => o).ToList()!;
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("api/TriageToolOption/GetOptions/ajax")]
        public async Task<IActionResult> Data()
        {
            var triagetooldocuments = await sharedContentRedis.GetDataAsync<TriagePageResponse>(Constants.TriagePages, "PUBLISHED");
            var triageFilters = await sharedContentRedis.GetDataAsync<TriageToolFilterResponse>(Constants.TriageToolFilters, "PUBLISHED");
            var sortedFilters = triageFilters.TriageToolFilter;

            List<TriageModelClass> modelClass = new List<TriageModelClass>();

            foreach (var filter in sortedFilters)
            {
                var subList = triagetooldocuments?.Page?.Where(doc => doc.TriageToolFilters.ContentItems.Any(tp => tp.DisplayText.Equals(filter.DisplayText, StringComparison.OrdinalIgnoreCase))).ToList();
                var pages = mapper.Map<List<TriagePages>>(subList);
                var filters = mapper.Map<TriageModelClass>(filter);
                filters.pages = pages;
                filters.filters = new List<TriageFilters>()
                {
                    new TriageFilters()
                    {
                        selected = false,
                        title = filters.title,
                        url = string.Empty,
                    },
                };
                modelClass.Add(filters);
            }

            return Json(modelClass);
        }
    }
}