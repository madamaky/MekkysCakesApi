using MediatR;
using MekkysCakes.Application.Features.Products.Commands.CreateProduct;
using MekkysCakes.Application.Features.Products.Commands.DeleteProduct;
using MekkysCakes.Application.Features.Products.Commands.UpdateProduct;
using MekkysCakes.Application.Features.Products.Queries.GetAllProducts;
using MekkysCakes.Application.Features.Products.Queries.GetAllThemes;
using MekkysCakes.Application.Features.Products.Queries.GetAllTypes;
using MekkysCakes.Application.Features.Products.Queries.GetProductById;
using MekkysCakes.Presentation.Attributes;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{

    /// <summary>
    /// Manages the product catalog, including search, types, themes, and admin controls.
    /// </summary>
    public class ProductController : ApiBaseController
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }


        /// <summary>
        /// Retrieves detailed information for a specific product by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product details if found.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var result = await _sender.Send(new GetProductByIdQuery(id));
            return HandleResult(result);
        }

        /// <summary>
        /// Retrieves a paginated list of active products based on optional filters. 
        /// Responses are cached to improve performance.
        /// </summary>
        /// <param name="queryParams">Filters (types, themes, search) and pagination parameters.</param>
        /// <returns>A paginated result containing matching products.</returns>
        [RedisCache]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _sender.Send(new GetAllProductsQuery(queryParams));
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a paginated list of all products, including inactive ones. Only accessible by administrators.
        /// </summary>
        /// <param name="queryParams">Filters (types, themes, search) and pagination parameters.</param>
        /// <returns>A paginated result containing all matching products.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("admin")]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProductsAdmin([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _sender.Send(new GetAllProductsQuery(queryParams));
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a list of all available product types.
        /// </summary>
        /// <returns>A collection of product types.</returns>
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var productTypes = await _sender.Send(new GetAllTypesQuery());
            return Ok(productTypes);
        }

        /// <summary>
        /// Retrieves a list of all available product themes.
        /// </summary>
        /// <returns>A collection of product themes.</returns>
        [HttpGet("themes")]
        public async Task<ActionResult<IEnumerable<ThemeDTO>>> GetAllThemes()
        {
            var productThemes = await _sender.Send(new GetAllThemesQuery());
            return Ok(productThemes);
        }

        /// <summary>
        /// Creates a new product in the catalog. Only accessible by administrators.
        /// </summary>
        /// <param name="command">The details of the new product to create.</param>
        /// <returns>True if the product was successfully created; otherwise, false.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("create")]
        public async Task<ActionResult<bool>> CreateProduct(CreateProductCommand command)
        {
            var result = await _sender.Send(command);
            return HandleResult(result);
        }

        /// <summary>
        /// Updates an existing product's details. Only accessible by administrators.
        /// </summary>
        /// <param name="id">The unique identifier of the product to update.</param>
        /// <param name="command">The updated details for the product.</param>
        /// <returns>True if the product was successfully updated; otherwise, false.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("update/{id}")]
        public async Task<ActionResult<bool>> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductCommand command)
        {
            // We need to ensure the route id matches the command id
            // Create a new command with the route id (records are immutable, so we use "with")
            var commandWithId = command with { Id = id };
            var result = await _sender.Send(commandWithId);
            return HandleResult(result);
        }

        /// <summary>
        /// Soft-deletes a product from the catalog. Only accessible by administrators.
        /// </summary>
        /// <param name="id">The identifier of the product to delete.</param>
        /// <returns>True if the product was successfully deleted; otherwise, false.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var result = await _sender.Send(new DeleteProductCommand(id));
            return HandleResult(result);
        }
    }
}
