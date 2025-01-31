using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.Models;
using DFC.Common.SharedContent.Pkg.Netcore.Constant;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using AppConstants = DFC.Common.SharedContent.Pkg.Netcore.Constant.ApplicationKeys;

namespace DFC.App.Triagetool.Controllers
{
    public class SitemapController : Controller
    {
        public const string SitemapViewCanonicalName = "sitemap";
        private const string ExpiryAppSettings = "Cms:Expiry";
        private readonly ILogger<SitemapController> logger;
        private readonly ISharedContentRedisInterface sharedContentRedis;
        private readonly IConfiguration configuration;
        private string status;
        private double expiryInHours = 4;

        public SitemapController(ILogger<SitemapController> logger, ISharedContentRedisInterface sharedContentRedis, IConfiguration configuration)
        {
            this.logger = logger;
            this.sharedContentRedis = sharedContentRedis;
            this.configuration = configuration;
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
        [Route("pages/sitemap")]
        public async Task<IActionResult> SitemapView()
        {
            var result = await Sitemap().ConfigureAwait(false);

            return result;
        }

        [HttpGet]
        [Route("/sitemap.xml")]
        public async Task<IActionResult> Sitemap()
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "PUBLISHED";
            }

            logger.LogInformation("Generating Sitemap");

            var sitemapUrlPrefix = $"{Request.GetBaseAddress()}{PagesController.RegistrationPath}";
            var sitemap = new Sitemap();
            var lookupResponse = await sharedContentRedis.GetDataAsyncWithExpiry<TriageLookupResponse>(ApplicationKeys.TriageToolLookup, status, expiryInHours);
            if (lookupResponse != null && lookupResponse.TriageLevelOne != null)
            {
                foreach (var levelOne in lookupResponse.TriageLevelOne)
                {
                    if (!string.IsNullOrEmpty(levelOne.Value))
                    {
                        if (lookupResponse?.TriageLevelTwo != null && levelOne.LevelTwo != null && levelOne.LevelTwo.ContentItems != null)
                        {
                            foreach (var levelTwo in levelOne.LevelTwo.ContentItems)
                            {
                                var matchedLevelTwo = lookupResponse.TriageLevelTwo.SingleOrDefault(x => x.ContentItemId == levelTwo.ContentItemId);
                                if (!string.IsNullOrEmpty(matchedLevelTwo?.Value))
                                {
                                    sitemap.Add(new SitemapLocation
                                    {
                                        Url = $"{sitemapUrlPrefix}/{levelOne.Value}/{matchedLevelTwo.Value}",
                                        Priority = 0.5,
                                        ChangeFrequency = SitemapLocation.ChangeFrequencies.Monthly,
                                    });
                                }
                            }
                        }
                    }
                }
            }

            if (!sitemap.Locations.Any())
            {
                logger.LogWarning("No Sitemap locations found");

                return NoContent();
            }

            var xmlString = sitemap.WriteSitemapToString();

            logger.LogInformation("Generated Sitemap");

            return Content(xmlString, MediaTypeNames.Application.Xml);
        }
    }
}