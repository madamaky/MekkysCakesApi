using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetDeliveryMethods
{
    public record GetDeliveryMethodsQuery() : IRequest<Result<IEnumerable<DeliveryMethodDTO>>>;

    public record DeliveryMethodDTO
    {
        public int Id { get; init; }
        public LocalizedString Name { get; init; } = new();
        public LocalizedString Description { get; init; } = new();
        public LocalizedString DeliveryTime { get; init; } = new();
        public decimal Price { get; init; }
    }
}
