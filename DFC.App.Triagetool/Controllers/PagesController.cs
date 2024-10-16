﻿using AutoMapper;
using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.Models;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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

        private const string ExpiryAppSettings = "Cms:Expiry";
        private readonly ILogger<PagesController> logger;
        private readonly IConfiguration configuration;
        private string status = string.Empty;
        private readonly AutoMapper.IMapper mapper;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        private double expiryInHours = 4;

        public PagesController(
            ILogger<PagesController> logger,
            IMapper mapper,
            ISharedContentRedisInterface sharedContentRedis,
            IConfiguration configuration)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.mapper = mapper;
            this.sharedContentRedis = sharedContentRedis;
            status = configuration.GetSection("contentMode:contentMode").Get<string>();
            if (this.configuration != null)
            {
                string expiryAppString = this.configuration.GetSection(ExpiryAppSettings).Get<string>();
                if (double.TryParse(expiryAppString, out var expiryAppStringParseResult))
                {
                    expiryInHours = expiryAppStringParseResult;
                }
            }
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
            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }

            var triagetooldocuments = await sharedContentRedis.GetDataAsyncWithExpiry<TriagePageResponse>(Constants.TriagePages, status, expiryInHours);
            var triageFilters = await sharedContentRedis.GetDataAsyncWithExpiry<TriageToolFilterResponse>(Constants.TriageToolFilters, status, expiryInHours);
            var sortedFilters = triageFilters?.TriageToolFilter.Select(x => x.DisplayText).OrderBy(o => o).ToList() !;

            if (article != null && sortedFilters != null)
            {
                article = FormatFilterCase(article, sortedFilters);
            }

            var subList = triagetooldocuments?.Page?.Where(doc => doc.TriageToolFilters.ContentItems.Any(tp => tp.DisplayText == article)).ToList();

            var triageToolModel = new TriageToolOptionViewModel
            {
                Title = article,
                Pages = subList,
            };

            try
            {
                var sharedhtml = await this.sharedContentRedis.GetDataAsyncWithExpiry<SharedHtml>(Constants.SpeakToAnAdviserSharedContent, status, expiryInHours);
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
            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }

            var triageFilters = await sharedContentRedis.GetDataAsyncWithExpiry<TriageToolFilterResponse>(Constants.TriageToolFilters, status, expiryInHours);

            var viewModel = new HeroBannerViewModel();

            if (triageFilters != null)
            {
                viewModel.Options = triageFilters?.TriageToolFilter.Select(x => x.DisplayText).OrderBy(o => o).ToList() !;

                if (viewModel.Options.Count > 0)
                {
                   viewModel.Selected = FormatFilterCase(article, viewModel.Options);
                }
            }

            return View(viewModel);
        }

        [HttpGet]
        [Route("api/TriageToolOption/GetOptions/ajax")]
        public async Task<IActionResult> Data()
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }

            var triagetooldocuments = await sharedContentRedis.GetDataAsyncWithExpiry<TriagePageResponse>(Constants.TriagePages, status, expiryInHours);
            var triageFilters = await sharedContentRedis.GetDataAsyncWithExpiry<TriageToolFilterResponse>(Constants.TriageToolFilters, status, expiryInHours);
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

        private string FormatFilterCase(string article, List<string> sortedFilters)
        {
            foreach (var option in sortedFilters)
            {
                if (string.Equals(option, article, StringComparison.CurrentCultureIgnoreCase))
                {
                    article = option;
                    break;
                }
            }

            return article;
        }
    }
}