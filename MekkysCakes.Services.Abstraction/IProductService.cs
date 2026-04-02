using MekkysCakes.Shared;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Services.Abstraction
{
    public interface IProductService
    {
        Task<Result<ProductDTO>> GetProductByIdAsync(int id);
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams);
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
        Task<IEnumerable<ThemeDTO>> GetAllThemesAsync();

        Task<Result<bool>> CreateProductAsync(CreateAndUpdateProductDTO createProductDTO);
        Task<Result<bool>> UpdateProductAsync(int id, CreateAndUpdateProductDTO updateProductDTO);
        Task<Result<bool>> DeleteProductAsync(int id);
    }
}