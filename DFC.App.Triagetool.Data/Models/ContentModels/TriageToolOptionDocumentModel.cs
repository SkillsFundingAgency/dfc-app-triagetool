using DFC.Compui.Cosmos.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Data.Models.ContentModels
{
    [ExcludeFromCodeCoverage]
    public class TriageToolOptionDocumentModel : DocumentModel
    {
        public const string DefaultPartitionKey = "triage-tool";

        public TriageToolOptionDocumentModel()
        {
            Filters = new List<TriageToolFilterDocumentModel>();
            Pages = new List<PageDocumentModel>();
            PageIds = new List<string>();
            FilterIds = new List<string>();
        }

        public override string? PartitionKey { get; set; } = DefaultPartitionKey;

        public Uri? Url { get; set; }

        public string? Title { get; set; }

        public List<string> FilterIds { get; set; }

        public List<string> PageIds { get; set; }

        public List<TriageToolFilterDocumentModel> Filters { get; set; }

        public List<PageDocumentModel> Pages { get; set; }
    }
}
