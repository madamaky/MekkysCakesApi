using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Services.Specifications
{
    internal class ProductsCountSpecification : BaseSpecification<Product, int>
    {
        public ProductsCountSpecification(ProductQueryParams queryParams)
            : base(ProductSpecificationHelper.GetProductCriteria(queryParams))
        {

        }
    }
}
