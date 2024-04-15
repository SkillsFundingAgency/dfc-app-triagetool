using DFC.App.Triagetool.ViewModels;
using System.Collections.Generic;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;

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
