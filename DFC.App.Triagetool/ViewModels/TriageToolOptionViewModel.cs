using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class TriageToolOptionViewModel
    {
        public TriageToolOptionViewModel()
        {
            Filters = new List<TriageToolFilterViewModel>();
            Pages = new List<TriagePage>();
            SelectedFilters = new List<string>();
        }

        public string? Title { get; set; }

        public string? DisplayText { get; set; }

        public List<TriageToolFilterViewModel> Filters { get; set; }

        public List<TriagePage> Pages { get; set; }

        public List<string> SelectedFilters { get; set; }

        public string? SharedContent { get; set; }
    }
}
