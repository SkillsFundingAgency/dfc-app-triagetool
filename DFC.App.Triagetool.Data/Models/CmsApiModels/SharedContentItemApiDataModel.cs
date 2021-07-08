using DFC.Content.Pkg.Netcore.Data.Models;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Data.Models.CmsApiModels
{
    [ExcludeFromCodeCoverage]
    public class SharedContentItemApiDataModel : BaseContentItemModel
    {
        public string? Content { get; set; }
    }
}
