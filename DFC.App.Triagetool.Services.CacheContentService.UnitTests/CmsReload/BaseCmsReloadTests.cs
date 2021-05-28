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
        private readonly Uri filterId = new Uri("Http://www.Filter.com");
        private readonly Uri pageId = new Uri("Http://www.page.com");
        private readonly Uri applicationViewId = new Uri("Http://www.application-view.com");
        private readonly Uri optionId = new Uri("Http://www.Option.com");

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
                    Url = pageId,
                },
            };
        }

        protected List<CmsApiSummaryItemModel> GetValidApplicationViewSummary()
        {
            return new List<CmsApiSummaryItemModel>
            {
                new CmsApiSummaryItemModel
                {
                    Url = applicationViewId,
                },
            };
        }

        protected CmsApiDataModel GetValidPage()
        {
            return new CmsApiDataModel
            {
                Url = pageId,
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
                        Url = filterId,
                        Title = "Test Filter",
                    },
                },
                FilterIds = new List<string>
                {
                    filterId.ToString(),
                },
                Title = "Test Page",
                Url = uri ?? optionId,
                Id = id ?? Guid.NewGuid(),
            };
        }

        protected PageDocumentModel GetPageDocumentModel()
        {
            return new PageDocumentModel
            {
                Filters = new List<string>
                {
                    filterId.ToString(),
                },
                Title = "test page",
                Uri = pageId,
            };
        }

    }
}
