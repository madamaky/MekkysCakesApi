using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Wishlists.Commands.AddItemToWishlist
{
    public record AddItemToWishlistCommand(int ProductId) : IRequest<Result<bool>>;
}
