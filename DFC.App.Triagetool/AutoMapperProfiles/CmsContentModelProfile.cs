using AutoMapper;
using DFC.App.Triagetool.Data.Models.ContentModels;
using DFC.App.Triagetool.Models;
using DFC.App.Triagetool.ViewModels;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Common;
using DFC.Common.SharedContent.Pkg.Netcore.Model.ContentItems;
using DFC.Common.SharedContent.Pkg.Netcore.Model.Response;
using System.Diagnostics.CodeAnalysis;

namespace DFC.App.Triagetool.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class CmsContentModelProfile : Profile
    {
        public CmsContentModelProfile()
        {
            CreateMap<TriageToolFilters, TriageModelClass>()
                .ForMember(d => d.title, s => s.MapFrom(w => w.DisplayText))
                .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TriagePage, TriagePages>()
             .ForMember(d => d.link, s => s.MapFrom(w => w.PageLocation.FullUrl))
             .ForMember(d => d.title, s => s.MapFrom(w => w.DisplayText))
             .ForMember(d => d.summary, s => s.MapFrom(w => w.TriageToolSummary.Html))
             .ForAllOtherMembers(d => d.Ignore());

            CreateMap<TriagePageResponse, TriageToolOptionViewModel>()
                  .ForAllOtherMembers(d => d.Ignore());

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
