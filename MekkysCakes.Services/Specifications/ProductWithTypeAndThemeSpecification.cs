using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Services.Specifications
{
    internal class ProductWithTypeAndThemeSpecification : BaseSpecification<Product, int>
    {
        public ProductWithTypeAndThemeSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductTheme);
        }

        public ProductWithTypeAndThemeSpecification(ProductQueryParams queryParams)
            : base(ProductSpecificationHelper.GetProductCriteria(queryParams))
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductTheme);

            switch (queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
    }
}
