using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
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

        public List<TriageLevelOne>? TriageLevelOnes { get; set; }

        public List<TriageLevelTwo>? TriageLevelTwos { get; set; }

        public List<string?>? FilterAdviceGroups { get; set; }

        public string? SelectedLevelOne { get; set; }

        public string? SelectedLevelTwo { get; set; }

        public string? SharedContent { get; set; }
    }
}
