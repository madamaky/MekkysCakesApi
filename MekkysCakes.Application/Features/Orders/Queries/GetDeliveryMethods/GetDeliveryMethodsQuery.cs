using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods
{
    public record GetDeliveryMethodsQuery() : IRequest<Result<IEnumerable<DeliveryMethodDTO>>>;
}
