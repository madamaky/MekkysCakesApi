using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        string Email,
        string BasketId,
        int DeliveryMethodId,
        AddressDTO Address
    ) : IRequest<Result<OrderToReturnDTO>>;
}
