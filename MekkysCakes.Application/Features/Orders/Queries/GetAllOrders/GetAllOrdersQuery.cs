using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetAllOrders
{
    public record GetAllOrdersQuery(string Email) : IRequest<Result<IEnumerable<OrderToReturnDTO>>>;
}
