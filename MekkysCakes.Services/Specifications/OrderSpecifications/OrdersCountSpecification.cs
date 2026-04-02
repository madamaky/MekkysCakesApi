using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Services.Specifications.OrderSpecifications
{
    internal class OrdersCountSpecification : BaseSpecification<Order, Guid>
    {
        public OrdersCountSpecification(OrderQueryParams queryParams)
            : base(OrderSpecificationHelper.GetOrderCriteria(queryParams))
        {
        }
    }
}
