using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Commands.CancelOrder
{
    public record CancelOrderCommand(Guid OrderId) : IRequest<Result<bool>>;
}
