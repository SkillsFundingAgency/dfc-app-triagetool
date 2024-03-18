using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using System.Collections.Generic;

namespace DFC.App.Triagetool.Models
{
    public class TriageClass
    {
        public TriageClass()
        {
            triageToolFilters = new List<TriageToolFilters>();
            triagePage = new List<TriagePage>();
        }

        public List<TriageToolFilters> triageToolFilters { get; set; }

        public List<TriagePage> triagePage { get; set; }
    }
}
