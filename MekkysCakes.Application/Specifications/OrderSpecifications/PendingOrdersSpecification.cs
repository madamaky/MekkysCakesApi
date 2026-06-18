using MekkysCakes.Domain.Entities.OrderModule;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public class PendingOrdersSpecification : BaseSpecification<Order, Guid>
    {
        public PendingOrdersSpecification(string email) : base(order => order.User.Email!.ToLower() == email.ToLower() && order.OrderStatus == OrderStatus.Pending)
        {
        }
    }
}
