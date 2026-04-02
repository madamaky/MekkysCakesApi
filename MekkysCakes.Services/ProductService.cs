using AutoMapper;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Services.Specifications;
using MekkysCakes.Shared;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.ProductDTOs;

namespace MekkysCakes.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var specification = new ProductWithTypeAndThemeSpecification(id);
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specification);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"Product With Id {id} Is Not Found");
            //throw new ProductNotFoundException(id);

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var repo = _unitOfWork.GetRepository<Product, int>();
            var specification = new ProductWithTypeAndThemeSpecification(queryParams);
            var products = await repo.GetAllAsync(specification);
            //if (products is null || !products.Any())

            var dataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);

            var countOfAllProducts = await repo.CountAsync(new ProductsCountSpecification(queryParams));

            return new PaginatedResult<ProductDTO>
            (
                queryParams.PageIndex,
                dataToReturn.Count(),
                countOfAllProducts,
                dataToReturn
            );
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
            => _mapper.Map<IEnumerable<TypeDTO>>(
                    await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync()
                );

        public async Task<IEnumerable<ThemeDTO>> GetAllThemesAsync()
            => _mapper.Map<IEnumerable<ThemeDTO>>(
                    await _unitOfWork.GetRepository<ProductTheme, int>().GetAllAsync()
                );

        public async Task<Result<bool>> CreateProductAsync(CreateAndUpdateProductDTO createProductDTO)
        {
            if (createProductDTO is null)
                return Error.Validation("Product.Invalid", "Provided Product Data Is Invalid");

            var type = await _unitOfWork.GetRepository<ProductType, int>().GetByIdAsync(createProductDTO.TypeId);
            if (type is null)
                return Error.NotFound("ProductType.NotFound", $"Product Type With Id {createProductDTO.TypeId} Is Not Found");

            var theme = await _unitOfWork.GetRepository<ProductTheme, int>().GetByIdAsync(createProductDTO.ThemeId);
            if (theme is null)
                return Error.NotFound("ProductTheme.NotFound", $"Product Theme With Id {createProductDTO.ThemeId} Is Not Found");

            var product = _mapper.Map<Product>(createProductDTO);

            await _unitOfWork.GetRepository<Product, int>().AddAsync(product);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Result<bool>> UpdateProductAsync(int id, CreateAndUpdateProductDTO updateProductDTO)
        {
            if (updateProductDTO is null)
                return Error.Validation("Product.Invalid", "Provided Product Data Is Invalid");

            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"Product With Id {id} Is Not Found");

            var type = await _unitOfWork.GetRepository<ProductType, int>().GetByIdAsync(updateProductDTO.TypeId);
            if (type is null)
                return Error.NotFound("ProductType.NotFound", $"Product Type With Id {updateProductDTO.TypeId} Is Not Found");

            var theme = await _unitOfWork.GetRepository<ProductTheme, int>().GetByIdAsync(updateProductDTO.ThemeId);
            if (theme is null)
                return Error.NotFound("ProductTheme.NotFound", $"Product Theme With Id {updateProductDTO.ThemeId} Is Not Found");

            //var updatedProduct = _mapper.Map<Product>(updateProductDTO);
            // updatedProduct.Id = id;

            product.Name = updateProductDTO.Name;
            product.Description = updateProductDTO.Description;
            product.PictureUrl = updateProductDTO.PictureUrl;
            product.Price = updateProductDTO.Price;
            product.ThemeId = updateProductDTO.ThemeId;
            product.TypeId = updateProductDTO.TypeId;

            _unitOfWork.GetRepository<Product, int>().Update(product);
            return await _unitOfWork.SaveChangesAsync();
        }

        public async Task<Result<bool>> DeleteProductAsync(int id)
        {
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"Product With Id {id} Is Not Found");

            _unitOfWork.GetRepository<Product, int>().Delete(product);
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}