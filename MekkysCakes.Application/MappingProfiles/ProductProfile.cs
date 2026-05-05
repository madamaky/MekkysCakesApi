using AutoMapper;
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
            CreateMap<Badge, BadgeDTO>();
            CreateMap<ProductType, TypeDTO>();
            CreateMap<ProductTheme, ThemeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ProductTheme, opt => opt.MapFrom(src => src.ProductTheme.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>())
                .ForMember(dest => dest.Badges, opt => opt.MapFrom(src => src.ProductBadges.Select(pb => pb.Badge.Name).ToList()));
            CreateMap<CreateProductCommand, Product>();
        }
    }
}
