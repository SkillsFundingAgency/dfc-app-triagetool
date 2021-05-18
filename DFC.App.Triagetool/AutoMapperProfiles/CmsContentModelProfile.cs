using AutoMapper;
using DFC.App.Triagetool.Data.Helpers;
using DFC.App.Triagetool.Data.Models.CmsApiModels;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.ViewModels;
using DFC.Content.Pkg.Netcore.Data.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

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
                .ForMember(d => d.Link, s => s.MapFrom(m => m.PageLocation))
                .ForMember(d => d.Summary, s => s.MapFrom(m => m.TriageToolSummary))
                .ForMember(d => d.Uri, s => s.MapFrom(m => m.Url))
                .ForMember(d => d.Filters, s => s.MapFrom(m => m.ContentItems.Where(x => x.ContentType == CmsContentKeyHelper.FilterTag).Select(x => x.Url)));

            CreateMap<CmsTriageToolFilterModel, TriageToolFilterDocumentModel>();

            CreateMap<TriageToolOptionItemModel, TriageToolOptionDocumentModel>()
                .ForMember(d => d.Pages, s => s.Ignore())
                .ForMember(d => d.FilterIds, s => s.MapFrom(m => m.ContentItems.Where(w => w.ContentType == CmsContentKeyHelper.FilterTag).Select(s => s.Url)))
                .ForMember(d => d.PageIds, s => s.Ignore())
                .ForMember(d => d.Filters, s => s.MapFrom(m => m.ContentItems.Where(w => w.ContentType == CmsContentKeyHelper.FilterTag).Select(s => s as CmsTriageToolFilterModel)))
                .ForMember(d => d.Url, s => s.MapFrom(m => m.Url))
                .ForMember(d => d.Title, s =>s.MapFrom(m => m.Title))
                .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TriageToolOptionDocumentModel, TriageToolOptionViewModel>()
                .ForMember(d => d.SelectedFilters, s => s.Ignore())
                .ForMember(d => d.SharedContent, s => s.Ignore());

            CreateMap<PageDocumentModel, TriageToolPageViewModel>();

            CreateMap<TriageToolFilterDocumentModel, TriageToolFilterViewModel>()
                .ForMember(d => d.Selected, s => s.Ignore());
        }
    }
}
