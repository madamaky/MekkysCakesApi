using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Orders.Commands.CreateDeliveryMethod
{
    public record CreateDeliveryMethodCommand(
        LocalizedString Name,
        LocalizedString Description,
        LocalizedString DeliveryTime,
        decimal Price
    ) : IRequest<Result<bool>>;
}
