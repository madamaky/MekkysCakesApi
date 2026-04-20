using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetOrderById
{
    public record GetOrderByIdQuery(string Email, Guid OrderId) : IRequest<Result<OrderToReturnDTO>>;
}
