using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Commands.DeleteDeliveryMethod
{
    public record DeleteDeliveryMethodCommand(int Id) : IRequest<Result<bool>>;
}
