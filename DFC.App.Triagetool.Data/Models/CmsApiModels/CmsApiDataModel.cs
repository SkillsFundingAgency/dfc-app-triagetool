using DFC.Content.Pkg.Netcore.Data.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Data.Models.CmsApiModels
{
    [ExcludeFromCodeCoverage]
    public class CmsApiDataModel : BaseContentItemModel
    {
        [JsonProperty("pagelocation_UrlName")]
        public string? CanonicalName { get; set; }

        [JsonProperty("pagelocation_DefaultPageForLocation")]
        public bool IsDefaultForPageLocation { get; set; }

        [JsonProperty("pagelocation_FullUrl")]
        public string? PageLocation { get; set; }

        public Guid? Version { get; set; }

        public bool UseInTriageTool { get; set; }

        public string? TriageToolSummary { get; set; }
    }
}
