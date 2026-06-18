using System.Linq.Expressions;
using MekkysCakes.Application.Features.Orders.Queries.GetAllOrdersForAdmin;
using MekkysCakes.Domain.Entities.OrderModule;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public static class OrderSpecificationHelper
    {
        public static Expression<Func<Order, bool>> GetOrderCriteria(OrderQueryParams queryParams)
            => o => (string.IsNullOrEmpty(queryParams.Email) || o.User.Email!.ToLower().Contains(queryParams.Email.ToLower()))
            && (!queryParams.OrderId.HasValue || o.Id == queryParams.OrderId.Value)
            && (!queryParams.OrderStatus.HasValue || o.OrderStatus == queryParams.OrderStatus.Value);
    }
}
