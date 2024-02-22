using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.Models;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.Common.SharedContent.Pkg.Netcore;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using DFC.Common.SharedContent.Pkg.Netcore.Interfaces;

namespace DFC.App.Triagetool.Controllers
{
    public class SitemapController : Controller
    {
        public const string SitemapViewCanonicalName = "sitemap";
        private readonly ILogger<SitemapController> logger;
        private readonly ISharedContentRedisInterface sharedContentRedis;

        public SitemapController(ILogger<SitemapController> logger, ISharedContentRedisInterface sharedContentRedis)
        {
            this.logger = logger;
            this.sharedContentRedis = sharedContentRedis;
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
            logger.LogInformation("Generating Sitemap");

            var sitemapUrlPrefix = $"{Request.GetBaseAddress()}{PagesController.RegistrationPath}";
            var sitemap = new Sitemap();
            var triagetooldocuments = await sharedContentRedis.GetDataAsync<TriageToolFilterResponse>("TraigeToolFilters/All");
                if (triagetooldocuments != null )
                {
                for (int i = 0; i < triagetooldocuments.TriageToolFilter.Count; i++)
                {
                    Common.SharedContent.Pkg.Netcore.Model.ContentItems.TriageToolFilter? contentPageModel = triagetooldocuments.TriageToolFilter[i];
                    sitemap.Add(new SitemapLocation
                    {
                        Url = $"{sitemapUrlPrefix}/{contentPageModel.DisplayText}",
                        Priority = 0.5,
                        ChangeFrequency = SitemapLocation.ChangeFrequencies.Monthly,
                    });
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