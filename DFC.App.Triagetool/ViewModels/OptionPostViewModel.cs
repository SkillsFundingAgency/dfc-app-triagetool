using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace DFC.App.Triagetool.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class OptionPostViewModel
    {
        [JsonProperty("triage-select")]
        public string? Title { get; set; }

        public string? Filters { get; set; }
    }
}
