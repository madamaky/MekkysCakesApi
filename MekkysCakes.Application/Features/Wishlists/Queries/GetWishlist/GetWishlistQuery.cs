using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.Features.Wishlists.Queries.GetWishlist
{
    public record GetWishlistQuery() : IRequest<Result<IEnumerable<WishlistItemDTO>>>;

    public record WishlistItemDTO(DateTime DateAdded, ProductDTO Product);
}
