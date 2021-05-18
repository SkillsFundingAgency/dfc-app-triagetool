using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class TriageToolFilterViewModel
    {
        public Uri? Url { get; set; }

        public string? Title { get; set; }

        public bool Selected { get; set; }
    }
}
