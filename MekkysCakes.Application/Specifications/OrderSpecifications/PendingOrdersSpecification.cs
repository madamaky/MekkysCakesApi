using MekkysCakes.Domain.Entities.OrderModule;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public class PendingOrdersSpecification : BaseSpecification<Order, Guid>
    {
        public PendingOrdersSpecification(string email) : base(order => order.UserEmail == email && order.OrderStatus == OrderStatus.Pending)
        {
        }
    }
}
