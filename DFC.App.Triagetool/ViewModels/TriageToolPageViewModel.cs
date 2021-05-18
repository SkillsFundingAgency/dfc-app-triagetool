using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class TriageToolPageViewModel
    {
        public TriageToolPageViewModel()
        {
            Filters = new List<Uri>();
        }

        public Uri? Uri { get; set; }

        public string? Summary { get; set; }

        public List<Uri> Filters { get; set; }

        public string? Link { get; set; }

        public string? Title { get; set; }
    }
}
