using MekkysCakes.Application.Features.Orders.Queries.GetAllOrdersForAdmin;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Shared.DTOs.OrderDTOs;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public class OrderSpecification : BaseSpecification<Order, Guid>
    {
        public OrderSpecification(OrderQueryParams queryParams)
            : base(OrderSpecificationHelper.GetOrderCriteria(queryParams))
        {
            AddThenInclude(order => order
                .Include(o => o.Items)
                .Include(o => o.DeliveryMethod)
            );

            switch (queryParams.Sort)
            {
                case OrderSortingOptions.OrderDateDescending:
                    AddOrderByDescending(o => o.OrderDate);
                    break;
                case OrderSortingOptions.OrderDateAscending:
                    AddOrderBy(o => o.OrderDate);
                    break;
                case OrderSortingOptions.TotalPriceDescending:
                    AddOrderByDescending(o => o.GetTotal());
                    break;
                case OrderSortingOptions.TotalPriceAscending:
                    AddOrderBy(o => o.GetTotal());
                    break;
                default:
                    AddOrderByDescending(o => o.OrderDate);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
        public OrderSpecification(string email) : base(order => order.UserEmail.ToLower() == email.ToLower())
        {
            AddThenInclude(order => order
                .Include(o => o.Items)
                .Include(o => o.DeliveryMethod)
            );

            AddOrderByDescending(O => O.OrderDate);
        }

        public OrderSpecification(Guid id, string email) : base(order => order.Id == id && (string.IsNullOrEmpty(email) || order.UserEmail.ToLower() == email.ToLower()))
        {
            AddThenInclude(order => order
                .Include(o => o.Items)
                .Include(o => o.DeliveryMethod)
            );
        }
    }
}
