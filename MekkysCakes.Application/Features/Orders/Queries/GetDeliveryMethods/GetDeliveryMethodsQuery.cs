using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods
{
    public record GetDeliveryMethodsQuery() : IRequest<Result<IEnumerable<DeliveryMethodDTO>>>;

    public record DeliveryMethodDTO(int Id, string Name, string Description, string DeliveryTime, decimal Price);
}
