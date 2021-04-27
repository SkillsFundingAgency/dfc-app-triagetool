using AutoMapper;
using DFC.App.Triagetool.Data.Models;
using DFC.App.Triagetool.Models;
using DFC.App.Triagetool.ViewModels;
using Microsoft.AspNetCore.Html;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DFC.App.Triagetool.AutoMapperProfiles
{
    [ExcludeFromCodeCoverage]
    public class SharedContentItemModelProfile : Profile
    {
        public SharedContentItemModelProfile()
        {
            CreateMap<SharedContentItemApiDataModel, SharedContentItemModel>()
                .ForMember(d => d.Id, s => s.MapFrom(a => a.ItemId))
                .ForMember(d => d.Etag, s => s.Ignore())
                .ForMember(d => d.ParentId, s => s.Ignore())
                .ForMember(d => d.TraceId, s => s.Ignore())
                .ForMember(d => d.PartitionKey, s => s.Ignore())
                .ForMember(d => d.LastReviewed, s => s.MapFrom(a => a.Published))
                .ForMember(d => d.LastCached, s => s.Ignore());

            CreateMap<SharedContentItemModel, IndexDocumentViewModel>();

            CreateMap<SharedContentItemModel, DocumentViewModel>()
                .ForMember(d => d.HtmlHead, s => s.MapFrom(a => a))
                .ForMember(d => d.Breadcrumb, s => s.Ignore())
                .ForMember(d => d.BodyViewModel, s => s.MapFrom(a => a));

            CreateMap<SharedContentItemModel, HtmlHeadViewModel>()
                .ForMember(d => d.CanonicalUrl, s => s.Ignore())
                .ForMember(d => d.Description, s => s.Ignore())
                .ForMember(d => d.Keywords, s => s.Ignore());

            CreateMap<SharedContentItemModel, BodyViewModel>()
                .ForMember(d => d.Body, s => s.MapFrom(a => new HtmlString(a.Content)));

            CreateMap<SharedContentItemModel, BreadcrumbItemModel>()
                .ForMember(d => d.Route, s => s.Ignore());

            CreateMap<BreadcrumbItemModel, BreadcrumbItemViewModel>()
                .ForMember(d => d.AddHyperlink, s => s.Ignore());
        }
    }
}
