using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Data.Models.ContentModels
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
    }
}
