using MekkysCakes.Presentation.Attributes;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{

    public class ProductController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(result);
        }

        [RedisCache]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("admin")]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProductsAdmin([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var productTypes = await _productService.GetAllTypesAsync();
            return Ok(productTypes);
        }

        [HttpGet("themes")]
        public async Task<ActionResult<IEnumerable<ThemeDTO>>> GetAllThemes()
        {
            var productThemes = await _productService.GetAllThemesAsync();
            return Ok(productThemes);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("create")]
        public async Task<ActionResult<bool>> CreateProduct(CreateAndUpdateProductDTO createProductDTO)
        {
            var result = await _productService.CreateProductAsync(createProductDTO);
            return HandleResult(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<bool>> UpdateProduct([FromRoute] int id, [FromBody] CreateAndUpdateProductDTO updateProductDTO)
        {
            var result = await _productService.UpdateProductAsync(id, updateProductDTO);
            return HandleResult(result);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var result = await _productService.DeleteProductAsync(id);
            return HandleResult(result);
        }
    }
}
