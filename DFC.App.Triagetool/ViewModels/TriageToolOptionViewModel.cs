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
            Pages = new List<TriageResultPage>();
            SelectedFilters = new List<string>();
        }

        public string? Title { get; set; }

        public string? DisplayText { get; set; }

        public List<TriageToolFilterViewModel> Filters { get; set; }

        public List<TriageResultPage> Pages { get; set; }

        public List<TriageToolResultGroupViewModel> ResultGroups { get; set; }

        public List<string> SelectedFilters { get; set; }

        public List<TriageLevelOne>? TriageLevelOnes { get; set; }

        public List<TriageLevelTwo>? TriageLevelTwos { get; set; }

        public List<FilterAdviceGroup>? FilterAdviceGroups { get; set; }

        public List<TriageResultTile>? TriageResultTiles { get; set; }

        public string? SelectedLevelOne { get; set; }

        public string? SelectedLevelTwo { get; set; }

        public string? SharedContent { get; set; }

        public string? LevelOneContentItemId { get;  set; }

        public List<TriageFilterAdviceGroupImage>? TriageFilterAdviceGroupImage { get;  set; }
    }
}
