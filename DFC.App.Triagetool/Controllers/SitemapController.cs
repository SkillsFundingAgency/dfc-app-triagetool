using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.Extensions;
using DFC.App.Triagetool.Models;
using DFC.Compui.Cosmos.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace DFC.App.Triagetool.Controllers
{
    public class SitemapController : Controller
    {
        public const string SitemapViewCanonicalName = "sitemap";

        private readonly ILogger<SitemapController> logger;
        private readonly IDocumentService<TriageToolOptionDocumentModel> triageToolDocumentService;

        public SitemapController(ILogger<SitemapController> logger, IDocumentService<TriageToolOptionDocumentModel> triageToolDocumentService)
        {
            this.logger = logger;
            this.triageToolDocumentService = triageToolDocumentService;
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
            var triageToolOptionModels = await triageToolDocumentService.GetAllAsync().ConfigureAwait(false);

            if (triageToolOptionModels != null && triageToolOptionModels.Any())
            {
                foreach (var contentPageModel in triageToolOptionModels)
                {
                    sitemap.Add(new SitemapLocation
                    {
                        Url = $"{sitemapUrlPrefix}/{contentPageModel.Title}",
                        Priority = 0.5,
                        ChangeFrequency = SitemapLocation.ChangeFrequencies.Monthly,
                    });
                }
            }

            if (!sitemap.Locations.Any())
            {
                return NoContent();
            }

            var xmlString = sitemap.WriteSitemapToString();

            logger.LogInformation("Generated Sitemap");

            return Content(xmlString, MediaTypeNames.Application.Xml);
        }
    }
}