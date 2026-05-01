using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Wishlists.Commands.RemoveItemFromWishlist
{
    public record RemoveItemFromWishlistCommand(int ProductId) : IRequest<Result<bool>>;
}
