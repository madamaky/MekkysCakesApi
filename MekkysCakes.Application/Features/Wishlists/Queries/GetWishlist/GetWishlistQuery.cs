using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.WishlistDTOs;

namespace MekkysCakes.Application.Features.Wishlists.Queries.GetWishlist
{
    public record GetWishlistQuery(string UserEmail) : IRequest<Result<IEnumerable<WishlistItemDTO>>>;
}
