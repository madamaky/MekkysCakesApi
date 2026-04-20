using MediatR;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllProducts
{
    /// <summary>
    /// Query to retrieve a paginated list of products with filtering/sorting.
    /// 
    /// Returns PaginatedResult (not wrapped in Result<>) because this query
    /// doesn't have failure cases — an empty list is still a valid result.
    /// </summary>
    public record GetAllProductsQuery(ProductQueryParams QueryParams) : IRequest<PaginatedResult<ProductDTO>>;
}
