using AutoMapper;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Services.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductType, TypeDTO>();
            CreateMap<ProductTheme, ThemeDTO>();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.ProductType.Name))
                .ForMember(dest => dest.ProductTheme, opt => opt.MapFrom(src => src.ProductTheme.Name))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CreateAndUpdateProductDTO, Product>();
        }
    }
}
