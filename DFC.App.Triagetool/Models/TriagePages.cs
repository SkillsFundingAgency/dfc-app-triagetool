using System.Collections.Generic;
using NHibernate.Mapping;

namespace DFC.App.Triagetool.Models
{
    public class TriagePages
    {
        public string? uri { get; set; }
        public string? title { get; set; }
        public string? summary { get; set; }

        public List<string> filters { get; set; }

        public string? link { get; set; }
    }
}
