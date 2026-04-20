using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.BasketDTOs;

namespace MekkysCakes.Application.Features.Baskets.Queries.GetBasket
{
    public record GetBasketQuery(string BasketId) : IRequest<Result<BasketDTO>>;
}
