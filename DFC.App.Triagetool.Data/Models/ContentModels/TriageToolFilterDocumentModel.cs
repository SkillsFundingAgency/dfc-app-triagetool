using System;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Data.Models.ContentModels
{
    [ExcludeFromCodeCoverage]
    public class TriageToolFilterDocumentModel
    {
        public string? Title { get; set; }

        public Uri? Url { get; set; }
    }
}
