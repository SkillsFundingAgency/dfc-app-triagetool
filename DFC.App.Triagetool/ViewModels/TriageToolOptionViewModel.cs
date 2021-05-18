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
            Pages = new List<TriageToolPageViewModel>();
            SelectedFilters = new List<string>();
        }

        public string? Title { get; set; }

        public List<TriageToolFilterViewModel> Filters { get; set; }

        public List<TriageToolPageViewModel> Pages { get; set; }

        public List<string> SelectedFilters { get; set; }

        public string? SharedContent { get; set; }
    }
}
