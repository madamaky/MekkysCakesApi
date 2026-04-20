using System.Linq.Expressions;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public static class OrderSpecificationHelper
    {
        public static Expression<Func<Order, bool>> GetOrderCriteria(OrderQueryParams queryParams)
            => o => (string.IsNullOrEmpty(queryParams.Email) || o.UserEmail.ToLower().Contains(queryParams.Email.ToLower()))
            && (!queryParams.OrderId.HasValue || o.Id == queryParams.OrderId.Value)
            && (!queryParams.OrderStatus.HasValue || o.OrderStatus == (OrderStatus)queryParams.OrderStatus);
    }
}
