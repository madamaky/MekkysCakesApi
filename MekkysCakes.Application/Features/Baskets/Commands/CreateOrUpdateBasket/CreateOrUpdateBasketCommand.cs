using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Application.Features.Baskets.Commands.CreateOrUpdateBasket
{
    public record CreateOrUpdateBasketCommand(
        string Email,
        ICollection<BasketItemDTO> Items
    ) : IRequest<Result<BasketDTO>>;
}
