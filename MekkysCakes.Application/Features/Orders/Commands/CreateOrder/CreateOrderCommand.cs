using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand
    (
        string UserName,
        string ContactEmail,
        string PhoneNumber,
        string Address,
        int DeliveryMethodId
    ) : IRequest<Result<OrderToReturnDTO>>;
}
