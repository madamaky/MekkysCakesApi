using MediatR;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Application.Features.Products.Queries.GetAllProducts
{
    public record GetAllProductsQuery(ProductQueryParams QueryParams) : IRequest<PaginatedResult<ProductDTO>>;

    public class ProductQueryParams : PaginatedQueryParams
    {
        public int? TypeId { get; set; }
        public int? ThemeId { get; set; }
        public List<int>? BadgeIds { get; set; }
        public string? Search { get; set; }
        public ProductSortingOptions Sort { get; set; }
    }

    public enum ProductSortingOptions
    {
        NameAsc = 1,
        NameDesc = 2,
        PriceAsc = 3,
        PriceDesc = 4,
    }
}
