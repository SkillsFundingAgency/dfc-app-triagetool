using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System;

namespace DFC.App.Triagetool.Models
{
    [ExcludeFromCodeCoverage]
    public class PageDocumentModel
    {
        public PageDocumentModel()
        {
            Filters = new List<string>();
        }

        public string Title { get; set; } = null!;

        public string Link { get; set; } = null!;

        public string Summary { get; set; } = null!;

        public Uri? Uri { get; set; } = null!;

        public List<string> Filters { get; set; }

        public DateTime? Published { get; set; }
    }
}
