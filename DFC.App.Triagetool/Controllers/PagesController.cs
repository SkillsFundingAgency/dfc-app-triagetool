﻿using AutoMapper;
using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.Models;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Constant;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems.SharedHtml;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Google.Protobuf.WellKnownTypes;
using FluentNHibernate.Conventions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Composition;
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
        [Route("pages/{triage-level-one?}/{triage-level-two?}/head")]
        public IActionResult Head([ModelBinder(Name = "triage-level-one")] string levelOne, [ModelBinder(Name = "triage-level-two")] string levelTwo)
        {
            logger.LogInformation($"{nameof(Head)} has been called");

            var viewModel = new HeadViewModel
            {
                Title = "Get relevant careers advice | National Careers Service",
                CanonicalUrl = new Uri($"{Request.GetBaseAddress()}{RegistrationPath}", UriKind.RelativeOrAbsolute),
                Description = "Get relevant careers advice",
            };

            logger.LogInformation($"{nameof(Head)} has returned content");

            return this.NegotiateContentResult(viewModel);
        }

        [HttpGet]
        [Route("pages/breadcrumb")]
        [Route("pages/{triage-select?}/breadcrumb")]
        [Route("pages/{triage-level-one?}/{triage-level-two?}/breadcrumb")]
        public IActionResult Breadcrumb([ModelBinder(Name = "triage-level-one")] string levelOne, [ModelBinder(Name = "triage-level-two")] string levelTwo)
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
                        Title = "Get relevant careers advice",
                    },
                },
            };
            logger.LogInformation($"{nameof(Breadcrumb)} has returned content");

            return View(viewModel);
        }

        [HttpGet]
        [Route("pages/bodytop")]
        [Route("pages/{triage-select?}/bodytop")]
        [Route("pages/{triage-level-one?}/{triage-level-two?}/bodytop")]
        public IActionResult BodyTop([ModelBinder(Name = "triage-level-one")] string levelOne, [ModelBinder(Name = "triage-level-two")] string levelTwo)
        {
            logger.LogWarning($"{nameof(BodyTop)} has returned no content");
            return NoContent();
        }

        [HttpGet]
        [Route("pages/body")]
        [Route("pages/{triage-select?}/body")]
        [Route("pages/{triage-level-one?}/{triage-level-two?}/body")]
        public async Task<IActionResult> Body([ModelBinder(Name = "triage-level-one")] string levelOne, [ModelBinder(Name = "triage-level-two")] string levelTwo,
            [ModelBinder(Name = "triage-select")] string multiLevels,
            [ModelBinder(Name = "filterAdviceGroupOptions")] List<string> filterAdviceGroupOptions,
            [ModelBinder(Name = "FilterAction")] TriageToolFilerAction? filterAction)
        {
            if (string.IsNullOrEmpty(levelOne) || string.IsNullOrEmpty(levelTwo))
            {
                if (string.IsNullOrEmpty(multiLevels))
                {
                    return View("Error");
                }
                else
                {
                    var selectedLevels = multiLevels.Split('|');
                    if (selectedLevels == null || selectedLevels.Length != 2)
                    {
                        return View("Error");
                    }

                    levelOne = selectedLevels[0];
                    levelTwo = selectedLevels[1];

                    if (string.IsNullOrEmpty(levelOne) || string.IsNullOrEmpty(levelTwo))
                    {
                        return View("Error");
                    }
                }
            }

            TriageToolOptionViewModel triageToolModel = await FilterResults(levelOne, levelTwo);
            if (filterAction.HasValue && filterAction == TriageToolFilerAction.ApplyFilters)
            {
                triageToolModel.SelectedFilters = filterAdviceGroupOptions == null ? new List<string>() : filterAdviceGroupOptions;
            }
            else
            {
                triageToolModel.SelectedFilters = new List<string>();
            }

            var filterQuery = new TriageFilterQuery { LevelOne = levelOne, LevelTwo = levelTwo, FilterAdviceGroup = triageToolModel.SelectedFilters };
            ApplyFilters(filterQuery, triageToolModel);

            return View(triageToolModel);
        }

        [HttpGet]
        [Route("pages/bodyfooter")]
        [Route("pages/{triage-select?}/bodyfooter")]
        [Route("pages/{triage-level-one?}/{triage-level-two?}/bodyfooter")]
        public IActionResult BodyFooter([ModelBinder(Name = "triage-level-one")] string levelOne, [ModelBinder(Name = "triage-level-two")] string levelTwo)
        {
            logger.LogWarning($"{nameof(BodyFooter)} has returned no content");

            return NoContent();
        }

        [HttpGet]
        [Route("pages/herobanner")]
        [Route("pages/{triage-select?}/herobanner")]
        [Route("pages/{triage-level-one?}/{triage-level-two?}/herobanner")]
        public IActionResult HeroBanner([ModelBinder(Name = "triage-level-one")] string? levelOne, [ModelBinder(Name = "triage-level-two")] string? levelTwo, [ModelBinder(Name = "triage-select")] string multiLevels)
        {
            if (string.IsNullOrEmpty(levelOne) || string.IsNullOrEmpty(levelTwo))
            {
                if (string.IsNullOrEmpty(multiLevels))
                {
                    return NoContent();
                }
                else
                {
                    var selectedLevels = multiLevels.Split('|');
                    if (selectedLevels == null || selectedLevels.Length != 2)
                    {
                        return NoContent();
                    }

                    levelOne = selectedLevels[0];
                    levelTwo = selectedLevels[1];

                    if (string.IsNullOrEmpty(levelOne) || string.IsNullOrEmpty(levelTwo))
                    {
                        return NoContent();
                    }
                }
            }

            var viewModel = new HeroBannerViewModel
            {
                SelectedLevelOne = levelOne,
                SelectedLevelTwo = levelTwo,
            };

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

            return Json(modelClass);
        }

        [HttpGet]
        [Route("api/triageresult/{appData}/ajax")]
        public async Task<IActionResult> TriageResult(string appData)
        {
            logger.LogInformation($"{nameof(TriageResult)} has been called");

            var filterQuery = System.Text.Json.JsonSerializer.Deserialize<TriageFilterQuery>(appData);
            var result = await FilterResults(filterQuery?.LevelOne, filterQuery?.LevelTwo);
            ApplyFilters(filterQuery, result);

            return PartialView("TriageToolPartialViews/TriageResult", result);
        }

        private static void ApplyFilters(TriageFilterQuery? filterQuery, TriageToolOptionViewModel? result)
        {
            if (result != null &&
                            filterQuery != null &&
                            filterQuery.FilterAdviceGroup != null &&
                            result.FilterAdviceGroups != null &&
                            filterQuery.FilterAdviceGroup.Any())
            {
                result.Pages = result.Pages.Where(x => x.FilterAdviceGroup != null &&
                                                       x.FilterAdviceGroup.ContentItems != null &&
                                                       x.FilterAdviceGroup.ContentItems.Any(y =>
                                                                            filterQuery.FilterAdviceGroup.Contains(y.ContentItemId))).ToList();
                result.FilterAdviceGroups = result.FilterAdviceGroups.Where(x => filterQuery.FilterAdviceGroup.Contains(x.ContentItemId)).ToList();
            }
        }

        private static void MatchFilterAdviceGroup(TriageLookupResponse? lookupResponse, TriageLevelTwo? levelTwo)
        {
            if (lookupResponse?.FilterAdviceGroup != null && levelTwo?.FilterAdviceGroup?.ContentItems != null)
            {
                foreach (var filterAdviceGroup in levelTwo.FilterAdviceGroup.ContentItems)
                {
                    var matchedFag = lookupResponse.FilterAdviceGroup.SingleOrDefault(x => x.ContentItemId == filterAdviceGroup.ContentItemId);
                    filterAdviceGroup.Title = matchedFag?.Title;
                    filterAdviceGroup.triageTileImage = matchedFag?.triageTileImage;
                }
            }
        }

        private static void MatchLevelTwo(TriageLookupResponse? lookupResponse, TriageLevelOne leveOne)
        {
            if (lookupResponse?.TriageLevelTwo != null && leveOne.LevelTwo != null && leveOne.LevelTwo.ContentItems != null)
            {
                foreach (var levelTwo in leveOne.LevelTwo.ContentItems)
                {
                    var matchedLevelTwo = lookupResponse.TriageLevelTwo.SingleOrDefault(x => x.ContentItemId == levelTwo.ContentItemId);

                    MatchFilterAdviceGroup(lookupResponse, matchedLevelTwo);

                    levelTwo.Value = matchedLevelTwo?.Value ?? string.Empty;
                    levelTwo.Title = matchedLevelTwo?.Title;
                    levelTwo.FilterAdviceGroup = matchedLevelTwo?.FilterAdviceGroup;
                }
            }
        }

        private async Task<TriageToolOptionViewModel> FilterResults(string? levelOne, string? levelTwo)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }

            var triageToolModel = new TriageToolOptionViewModel();

            var triageResultPages = await sharedContentRedis.GetDataAsyncWithExpiry<TriageResultPageResponse>(ApplicationKeys.TriageResults, status, expiryInHours);
            var lookupResponse = await sharedContentRedis.GetDataAsyncWithExpiry<TriageLookupResponse>(ApplicationKeys.TriageToolLookup, status, expiryInHours);

            if (lookupResponse != null)
            {
                if (lookupResponse.TriageLevelOne != null)
                {
                    foreach (var leveOne in lookupResponse.TriageLevelOne)
                    {
                        leveOne.Value = leveOne.Value ?? string.Empty;
                        MatchLevelTwo(lookupResponse, leveOne);
                    }
                }

                triageToolModel.SelectedLevelOne = levelOne;
                triageToolModel.SelectedLevelTwo = levelTwo;

                //Selected levelone content Item Id
                var levelOneContentItemId = lookupResponse?.TriageLevelOne?.SingleOrDefault(x => x.Value == levelOne)?.ContentItemId;
                triageToolModel.LevelOneContentItemId = levelOneContentItemId;

                //Select list of leveltwo contentItem ids for selected levelone
                // and pick contentItemId for the selected level two
                var levelTwoIds = lookupResponse?.TriageLevelOne?.SingleOrDefault(x => x.Value == levelOne)?.LevelTwo?.ContentItems?.Select(x => x.ContentItemId).ToList();
                var levelTwoContentItemId = lookupResponse?.TriageLevelTwo?.SingleOrDefault(x => x.Value == levelTwo && levelTwoIds != null && levelTwoIds.Contains(x.ContentItemId))?.ContentItemId;

                FilterPages(triageToolModel, triageResultPages, levelOneContentItemId, levelTwoContentItemId);
                FilterApplicationView(triageToolModel, triageResultPages, levelTwoContentItemId);
                FilterApprenticeshipLink(triageToolModel, triageResultPages, levelTwoContentItemId);

                triageToolModel.Pages = triageToolModel.Pages.OrderBy(x => x.TriageOrdinal).ToList();
                triageToolModel.TriageFilterAdviceGroupImage = triageResultPages?.TriageFilterAdviceGroupImage;

                //Filter advice groups based on level one and level two
                if (lookupResponse?.TriageLevelOne != null)
                {
                    var selectedLevelOne = lookupResponse.TriageLevelOne.SingleOrDefault(l1 => l1.Value == levelOne);
                    var leveTwos = selectedLevelOne?.LevelTwo?.ContentItems;
                    triageToolModel.FilterAdviceGroups = leveTwos?.SingleOrDefault(x => x.Value == levelTwo)?.FilterAdviceGroup?.ContentItems;
                    triageToolModel.AllFilterAdviceGroups = leveTwos?.SingleOrDefault(x => x.Value == levelTwo)?.FilterAdviceGroup?.ContentItems;
                    triageToolModel.TriageResultTiles = triageResultPages?.TriageResultTile;
                }
            }

            return triageToolModel;
        }

        private void FilterApprenticeshipLink(TriageToolOptionViewModel triageToolModel, TriageResultPageResponse? triageResultPages, string? levelTwoContentItemId)
        {
            if (triageResultPages?.ApprenticeshipLink != null)
            {
                //Filter triage ApprenticeshipLink based on selected level one and level two content Item ids
                triageToolModel.Pages.AddRange(triageResultPages.ApprenticeshipLink.Where(x => x.TriageLevelTwo?.ContentItems != null &&
                                                                                              x.TriageLevelTwo.ContentItems.Any(x => x.ContentItemId == levelTwoContentItemId)));
            }
        }

        private void FilterApplicationView(TriageToolOptionViewModel triageToolModel, TriageResultPageResponse? triageResultPages, string? levelTwoContentItemId)
        {
            if (triageResultPages?.ApplicationView != null)
            {
                //Filter triage application views based on selected level one and level two content Item ids
                triageToolModel.Pages.AddRange(triageResultPages.ApplicationView.Where(x => x.TriageLevelTwo?.ContentItems != null &&
                                                                                           x.TriageLevelTwo.ContentItems.Any(x => x.ContentItemId == levelTwoContentItemId)));
            }
        }

        private void FilterPages(TriageToolOptionViewModel triageToolModel, TriageResultPageResponse? triageResultPages, string? levelOneContentItemId, string? levelTwoContentItemId)
        {
            if (triageResultPages?.Page != null)
            {
                //Filter triage pages based on selected level one and level two content Item ids
                triageToolModel.Pages = triageResultPages.Page.Where(x => x.TriageLevelTwo?.ContentItems != null &&
                                                                          x.TriageLevelTwo.ContentItems.Any(x => x.ContentItemId == levelTwoContentItemId) &&
                                                                          x.TriageLevelOne?.ContentItems != null &&
                                                                          x.TriageLevelOne.ContentItems.Any(x => x.ContentItemId == levelOneContentItemId)).ToList();
                triageToolModel.SharedContent = triageResultPages.SharedContent?.FirstOrDefault()?.Content.Html;
            }
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