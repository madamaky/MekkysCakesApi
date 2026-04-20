using AutoMapper;
using MekkysCakes.Domain.Entities.WishlistModule;
using MekkysCakes.Shared.DTOs.WishlistDTOs;

namespace MekkysCakes.Application.MappingProfiles
{
    public class WishlistProfile : Profile
    {
        public WishlistProfile()
        {
            CreateMap<WishlistItem, WishlistItemDTO>();
        }
    }
}
