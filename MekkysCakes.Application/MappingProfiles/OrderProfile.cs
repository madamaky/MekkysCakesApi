using AutoMapper;
using MekkysCakes.Application.Extensions;
using MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.MappingProfiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderToReturnDTO>()
                .ForMember(dest => dest.OrdersStatus, opt => opt.MapFrom(src => src.OrderStatus.ToString()))
                .ForMember(dest => dest.DeliveryMethodName, opt => opt.MapFrom(src => src.DeliveryMethod.Translations.ToLocalized(t => t.Name)));

            CreateMap<OrderItem, OrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<OrderItemPictureUrlResolver>());

            CreateMap<DeliveryMethod, DeliveryMethodDTO>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.Name)))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.Description)))
                .ForMember(dest => dest.DeliveryTime, opt => opt.MapFrom(src => src.Translations.ToLocalized(t => t.DeliveryTime)));
        }   
    }
}
