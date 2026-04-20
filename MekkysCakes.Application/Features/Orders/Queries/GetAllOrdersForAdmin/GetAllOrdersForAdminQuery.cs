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
}
