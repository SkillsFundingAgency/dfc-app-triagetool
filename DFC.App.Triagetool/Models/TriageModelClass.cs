using System.Collections.Generic;

namespace DFC.App.Triagetool.Models
{
    public class TriageModelClass
    {
        public string? title {  get; set; }
        public List<TriageFilters> filters { get; set; }

        public List<TriagePages> pages { get; set; }
    }
}
