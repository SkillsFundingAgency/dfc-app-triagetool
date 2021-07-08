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

        public List<string> Options { get; set; }
    }
}
