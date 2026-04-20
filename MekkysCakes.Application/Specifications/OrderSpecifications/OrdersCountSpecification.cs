using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public class OrdersCountSpecification : BaseSpecification<Order, Guid>
    {
        public OrdersCountSpecification(OrderQueryParams queryParams)
            : base(OrderSpecificationHelper.GetOrderCriteria(queryParams))
        {
        }
    }
}
