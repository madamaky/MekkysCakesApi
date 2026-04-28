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
using Microsoft.AspNetCore.Http;

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
        /// <remarks>
        /// Returns full product details including pricing, stock, and metadata.
        /// 
        /// Sample request:
        ///
        ///     GET /api/product/42
        ///
        /// </remarks>
        /// <response code="200">Returns the product details</response>
        /// <response code="404">Product with the given ID was not found</response>
        /// <response code="400">Invalid ID supplied</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <remarks>
        /// Use to populate the main shop view for regular users.
        /// 
        /// Sample request:
        ///
        ///     GET /api/product?pageIndex=1&amp;pageSize=10&amp;sort=priceAsc
        ///
        /// </remarks>
        /// <response code="200">Returns a paginated result containing matching products</response>
        [RedisCache]
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<ProductDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _sender.Send(new GetAllProductsQuery(queryParams));
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a paginated list of all products, including inactive ones. Only accessible by administrators.
        /// </summary>
        /// <param name="queryParams">Filters (types, themes, search) and pagination parameters.</param>
        /// <remarks>
        /// For internal administration mapping in back-office.
        /// 
        /// Sample request:
        ///
        ///     GET /api/product/admin?pageIndex=1&amp;pageSize=50
        ///
        /// </remarks>
        /// <response code="200">Returns a paginated result containing all matching products</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access (not an admin)</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("admin")]
        [ProducesResponseType(typeof(PaginatedResult<ProductDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProductsAdmin([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _sender.Send(new GetAllProductsQuery(queryParams));
            return Ok(products);
        }

        /// <summary>
        /// Retrieves a list of all available product types.
        /// </summary>
        /// <remarks>
        /// Exposes type criteria for creating dynamic UI filters.
        /// 
        /// Sample request:
        ///
        ///     GET /api/product/types
        ///
        /// </remarks>
        /// <response code="200">Returns a collection of product types</response>
        [HttpGet("types")]
        [ProducesResponseType(typeof(IEnumerable<TypeDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var productTypes = await _sender.Send(new GetAllTypesQuery());
            return Ok(productTypes);
        }

        /// <summary>
        /// Retrieves a list of all available product themes.
        /// </summary>
        /// <remarks>
        /// Exposes theme criteria for creating dynamic UI filters.
        /// 
        /// Sample request:
        ///
        ///     GET /api/product/themes
        ///
        /// </remarks>
        /// <response code="200">Returns a collection of product themes</response>
        [HttpGet("themes")]
        [ProducesResponseType(typeof(IEnumerable<ThemeDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ThemeDTO>>> GetAllThemes()
        {
            var productThemes = await _sender.Send(new GetAllThemesQuery());
            return Ok(productThemes);
        }

        /// <summary>
        /// Creates a new product in the catalog. Only accessible by administrators.
        /// </summary>
        /// <param name="command">The details of the new product to create.</param>
        /// <remarks>
        /// Ingests all core setup data for a single physical product.
        /// 
        /// Sample request:
        ///
        ///     POST /api/product/create
        ///     {
        ///        "name": "Chocolate Cake",
        ///        "price": 20.00,
        ///        ...
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns true if the product was successfully created</response>
        /// <response code="400">Invalid product payload provided</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access (not an admin)</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("create")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        /// <remarks>
        /// Applies partial or complete modifications to an existing catalog product.
        /// 
        /// Sample request:
        ///
        ///     PUT /api/product/update/42
        ///     {
        ///        "name": "Updated Cake Name",
        ///        ...
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns true if the product was successfully updated</response>
        /// <response code="400">Mismatch in body ID and URI ID or invalid structure</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access (not an admin)</response>
        /// <response code="404">Product ID to update not found</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPut("update/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <remarks>
        /// Removes the product from search and general listings but keeps it historically.
        /// 
        /// Sample request:
        ///
        ///     DELETE /api/product/delete/42
        ///
        /// </remarks>
        /// <response code="200">Returns true if the product was successfully removed</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access (not an admin)</response>
        /// <response code="404">Target product ID not found</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var result = await _sender.Send(new DeleteProductCommand(id));
            return HandleResult(result);
        }
    }
}
