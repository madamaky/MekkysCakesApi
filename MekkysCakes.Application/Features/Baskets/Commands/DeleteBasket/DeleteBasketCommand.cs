using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Baskets.Commands.DeleteBasket
{
    public record DeleteBasketCommand(string BasketId) : IRequest<Result<bool>>;
}
