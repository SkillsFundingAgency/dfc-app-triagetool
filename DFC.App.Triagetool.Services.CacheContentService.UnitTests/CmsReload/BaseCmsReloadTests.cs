using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.Content.Pkg.Netcore.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.Services.CacheContentService.UnitTests.CmsReload
{
    [ExcludeFromCodeCoverage]
    public abstract class BaseCmsReloadTests
    {
        public Uri FilterId = new Uri("Http://www.Filter.com");
        public Uri PageId = new Uri("Http://www.page.com");
        public Uri OptionId = new Uri("Http://www.Option.com");

        protected List<TriageToolOptionSummaryModel> GetValidCmsOptionSummary()
        {
            var options = new List<TriageToolOptionSummaryModel>
            {
                new TriageToolOptionSummaryModel
                {
                    Title = "test",
                    Url = new Uri("https://Test.com"),
                },
            };

            return options;
        }

        protected List<CmsApiSummaryItemModel> GetValidPagesSummary()
        {
            return new List<CmsApiSummaryItemModel>
            {
                new CmsApiSummaryItemModel
                {
                    Url = PageId,
                },
            };
        }

        protected CmsApiDataModel GetValidPage()
        {
            return new CmsApiDataModel
            {
                Url = PageId,
                UseInTriageTool = true,
            };
        }

        protected TriageToolOptionItemModel GetValidOptionItem()
        {
            return new TriageToolOptionItemModel
            {
                ContentType = "TriageToolOption",
                ContentItems = new List<IBaseContentItemModel>
                {
                    new CmsTriageToolFilterModel
                    {
                        Title = "test filter",
                        Url = new Uri("http://www.TestFilter.com"),
                    },
                },
                Title = "Test option",
            };
        }

        protected TriageToolOptionDocumentModel GetTriageToolOptionDocumentModel(Guid? id = null, Uri? uri = null)
        {
            return new TriageToolOptionDocumentModel
            {
                Filters = new List<TriageToolFilterDocumentModel>
                {
                    new TriageToolFilterDocumentModel
                    {
                        Url = FilterId,
                        Title = "Test Filter",
                    },
                },
                FilterIds = new List<string>
                {
                    FilterId.ToString(),
                },
                Title = "Test Page",
                Url = uri ?? OptionId,
                Id = id ?? Guid.NewGuid(),
            };
        }

        protected PageDocumentModel GetPageDocumentModel()
        {
            return new PageDocumentModel
            {
                Filters = new List<string>
                {
                    FilterId.ToString(),
                },
                Title = "test page",
                Uri = PageId,
            };
        }

    }
}
