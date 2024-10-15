using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class TriageToolResultGroupViewModel
    { 
        public string? FilterAdviceGroup { get; set; }

        public List<TriageResultPage>? ResultPages { get; set; }
    }
}
