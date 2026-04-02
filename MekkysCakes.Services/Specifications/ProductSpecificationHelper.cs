using System.Linq.Expressions;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Services.Specifications
{
    internal static class ProductSpecificationHelper
    {
        public static Expression<Func<Product, bool>> GetProductCriteria(ProductQueryParams queryParams)
            => p => (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value)
            && (!queryParams.ThemeId.HasValue || p.ThemeId == queryParams.ThemeId.Value)
            && (string.IsNullOrEmpty(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search.ToLower()));
    }
}
