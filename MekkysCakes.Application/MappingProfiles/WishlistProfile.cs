using AutoMapper;
using MekkysCakes.Domain.Entities.WishlistModule;
using MekkysCakes.Application.Features.Wishlists.Queries.GetWishlist;

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
