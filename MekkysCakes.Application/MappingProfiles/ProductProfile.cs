using AutoMapper;
using MekkysCakes.Application.Extensions;
using MekkysCakes.Application.Features.Products.Commands.CreateProduct;
using MekkysCakes.Application.Features.Products.Queries.GetAllBadges;
using MekkysCakes.Application.Features.Products.Queries.GetAllThemes;
using MekkysCakes.Application.Features.Products.Queries.GetAllTypes;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Name,         opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.Name)))
                .ForMember(dest => dest.Description,  opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.Description)))
                .ForMember(dest => dest.ProductType,  opt => opt.MapFrom(src => src.ProductType.Translations.ToLocalized(t => t.Name)))
                .ForMember(dest => dest.ProductTheme, opt => opt.MapFrom(src => src.ProductTheme.Translations.ToLocalized(t => t.Name)))
                .ForMember(dest => dest.Badges,       opt => opt.MapFrom(src => src.ProductBadges.Select(pb => pb.Badge.Translations.ToLocalized(t => t.Name)).ToList()))
                .ForMember(dest => dest.PictureUrl,   opt => opt.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Badge, BadgeDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.Name)));

            CreateMap<ProductType, TypeDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.Name)));

            CreateMap<ProductTheme, ThemeDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.Name)));

            CreateMap<CreateProductCommand, Product>()
                .ForMember(dest => dest.Translations, opt => opt.Ignore())
                .ForMember(dest => dest.ProductBadges, opt => opt.Ignore());
        }
    }
}
