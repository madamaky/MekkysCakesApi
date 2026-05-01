using MediatR;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.OrderDTOs;

namespace MekkysCakes.Application.Features.Orders.Queries.GetAllOrdersForAdmin
{
    /// <summary>
    /// Query to retrieve a paginated list of orders for admin with filtering/sorting.
    /// 
    /// Returns PaginatedResult (not wrapped in Result&lt;&gt;) because this query
    /// doesn't have failure cases — an empty list is still a valid result.
    /// </summary>
    public record GetAllOrdersForAdminQuery(OrderQueryParams QueryParams) : IRequest<PaginatedResult<OrderToReturnDTO>>;

    public class OrderQueryParams
    {
        public string? Email { get; set; }
        public Guid? OrderId { get; set; } = null;

        public OrderStatusDTO? OrderStatus { get; set; }
        public OrderSortingOptions Sort { get; set; }

        private int _pageIndex = 1;
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value < 1 ? 1 : value; }
        }

        private const int DefaultPageSize = 5;
        private const int MaxPageSize = 10;
        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value < 1 ? DefaultPageSize : (value > MaxPageSize ? MaxPageSize : value); }
        }
    }

    public enum OrderSortingOptions
    {
        OrderDateDescending = 0,
        OrderDateAscending = 1,
        TotalPriceDescending = 2,
        TotalPriceAscending = 3,
    }
}
