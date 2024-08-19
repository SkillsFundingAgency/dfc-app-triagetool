using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class HeroBannerViewModel
    {
        public HeroBannerViewModel()
        {
            Options = new List<string>();
        }

        public List<TriageLevelOne>? TriageLevelOnes { get; set; }

        public List<TriageLevelTwo>? TriageLevelTwos { get; set; }
        public List<string?>? FilterAdviceGroups { get; set; }

        public string? SelectedLevelOne { get; set; }

        public string? SelectedLevelTwo { get; set; }

        public List<string> Options { get; set; }

        public string? Selected { get; set; }
    }
}
