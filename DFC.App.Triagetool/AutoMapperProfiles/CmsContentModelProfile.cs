using System;
using AutoMapper;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.ViewModels;
using DFC.Content.Pkg.Netcore.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using DFC.App.Triagetool.Models;

namespace DFC.App.Triagetool.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class CmsContentModelProfile : Profile
    {
        public CmsContentModelProfile()
        {
            CreateMap<LinkDetails, CmsTriageToolFilterModel>()
                .ForMember(d => d.Url, s => s.Ignore())
                .ForMember(d => d.ItemId, s => s.Ignore())
                .ForMember(d => d.Title, s => s.Ignore())
                .ForMember(d => d.Published, s => s.Ignore())
                .ForMember(d => d.CreatedDate, s => s.Ignore())
                .ForMember(d => d.Links, s => s.Ignore())
                .ForMember(d => d.ContentLinks, s => s.Ignore())
                .ForMember(d => d.ContentItems, s => s.Ignore())
                .ForMember(d => d.Justify, s => s.Ignore());

            CreateMap<CmsApiDataModel, PageDocumentModel>()
                .ForMember(d => d.Link, s => s.MapFrom(m => m.PageLocation ?? m.FullUrl))
                .ForMember(d => d.Summary, s => s.MapFrom(m => m.TriageToolSummary))
                .ForMember(d => d.Uri, s => s.MapFrom(m => m.Url))
                .ForMember(d => d.Filters, s => s.MapFrom(m => m.ContentItems.Where(x => x.ContentType != null && x.ContentType.Equals(CmsContentKeyHelper.FilterTag, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.Url)));

            CreateMap<CmsTriageToolFilterModel, TriageToolFilterDocumentModel>();

            //CreateMap<TriageToolOptionItemModel, TriageToolOptionDocumentModel>()
            //    .ForMember(d => d.Id, s => s.MapFrom(m => m.ItemId))
            //    .ForMember(d => d.Pages, s => s.Ignore())
            //    .ForMember(d => d.FilterIds, s => s.MapFrom(m => m.ContentItems.Where(w => w.ContentType != null && w.ContentType.Equals(CmsContentKeyHelper.FilterTag, StringComparison.InvariantCultureIgnoreCase)).Select(s => s.Url)))
            //    .ForMember(d => d.PageIds, s => s.Ignore())
            //    .ForMember(d => d.Filters, s => s.MapFrom(m => m.ContentItems.Where(w => w.ContentType != null && w.ContentType.Equals(CmsContentKeyHelper.FilterTag, StringComparison.InvariantCultureIgnoreCase)).Select(s => s as CmsTriageToolFilterModel)))
            //    .ForMember(d => d.Url, s => s.MapFrom(m => m.Url))
            //    .ForMember(d => d.Title, s => s.MapFrom(m => m.Title))
            //    .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TriageToolOptionDocumentModel, TriageToolOptionViewModel>()
                .ForMember(d => d.SelectedFilters, s => s.Ignore())
                .ForMember(d => d.SharedContent, s => s.Ignore())
             .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TriageToolFilters, TriageModelClass>()
                .ForMember(d => d.title, s => s.MapFrom(w => w.DisplayText))
                .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TriagePage, TriagePages>()
             //.ForMember(d => d.Filters, s => s.MapFrom(w => w.TriageToolFilters.ContentItems))
             .ForMember(d => d.link, s => s.MapFrom(w => w.PageLocation.FullUrl))
             .ForMember(d => d.title, s => s.MapFrom(w => w.DisplayText))
             .ForMember(d => d.summary, s => s.MapFrom(w => w.TriageToolSummary.Html))
             .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TriagePageResponse, TriageToolOptionViewModel>()
                  .ForAllOtherMembers(d => d.Ignore());

            CreateMap<PageDocumentModel, TriageToolPageViewModel>();

            CreateMap<TriageToolFilterDocumentModel, TriageToolFilterViewModel>()
                .ForMember(d => d.Selected, s => s.Ignore());

            CreateMap<TriagePage, PageDocumentModel>()
            .ForMember(d => d.Link, s => s.MapFrom(m => m.PageLocation.FullUrl))
            .ForMember(d => d.Summary, s => s.MapFrom(m => m.TriageToolSummary))
            .ForMember(d => d.Uri, s => s.MapFrom(m => m.PageLocation.FullUrl))
            .ForMember(d => d.Title, s => s.MapFrom(m => m.DisplayText))
            .ForMember(d => d.Filters, s => s.MapFrom(m => m.TriageToolFilters.ContentItems))
            .ForAllOtherMembers(d => d.Ignore());




        }
    }
}
