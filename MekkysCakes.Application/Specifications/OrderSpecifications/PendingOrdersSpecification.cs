using MekkysCakes.Domain.Entities.OrderModule;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public class PendingOrdersSpecification : BaseSpecification<Order, Guid>
    {
        public PendingOrdersSpecification(string email) : base(order => order.UserEmail == email && order.OrderStatus == OrderStatus.Pending)
        {
            // Include items just in case the database needs to cascade delete them 
            // (though your EF Core Cascade Delete configuration usually handles this)
            //AddInclude(order => order.Items);
        }
    }
}
