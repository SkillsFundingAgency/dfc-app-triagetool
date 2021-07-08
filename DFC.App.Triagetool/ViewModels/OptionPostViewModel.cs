using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class OptionPostViewModel
    {
        [ModelBinder(Name = "triage-select")]
        public string? Title { get; set; }

        public string? Filters { get; set; }
    }
}
