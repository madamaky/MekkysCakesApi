using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Application.Features.Orders.Queries.GetAllOrdersForAdmin;

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
