using DFC.Content.Pkg.Netcore.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Data.Models.CmsApiModels
{
    [ExcludeFromCodeCoverage]
    public class CmsTriageToolFilterModel : BaseContentItemModel
    {
        public int? Justify { get; set; }

        public string? Alignment { get; set; }

        public int? Ordinal { get; set; }

        public int? Size { get; set; }

        public string? Href { get; set; }
    }
}
