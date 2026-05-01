using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods
{
    public record GetDeliveryMethodsQuery() : IRequest<Result<IEnumerable<DeliveryMethodDTO>>>;

    public record DeliveryMethodDTO(int Id, string ShortName, string Description, string DeliveryTime, decimal Price);
}
