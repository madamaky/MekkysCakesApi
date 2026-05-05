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

    public class ProductQueryParams
    {
        public int? TypeId { get; set; }
        public int? ThemeId { get; set; }
        public List<int>? BadgeIds { get; set; }
        public string? Search { get; set; }
        public ProductSortingOptions Sort { get; set; }

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

    public enum ProductSortingOptions
    {
        NameAsc = 1,
        NameDesc = 2,
        PriceAsc = 3,
        PriceDesc = 4,
    }
}
