using AutoMapper;
using MekkysCakes.Domain.Entities.BasketModule;
using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Services.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<CustomerBasket, BasketDTO>().ReverseMap();
            CreateMap<BasketItem, BasketItemDTO>().ReverseMap();
        }
    }
}
