using MekkysCakes.Application.Features.Products.Queries.GetAllBadges;
using MekkysCakes.Application.Features.Products.Queries.GetAllProducts;
using MekkysCakes.Application.Features.Products.Queries.GetAllThemes;
using MekkysCakes.Application.Features.Products.Queries.GetAllTypes;
using MekkysCakes.Application.Features.Products.Queries.GetProductById;
using MekkysCakes.Presentation.Attributes;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class ProductController : ApiBaseController
    {
        /// <summary> Get product by id </summary>
        /// <remarks> Retrieves detailed information for a specific product by its ID. </remarks>
        /// <response code="200">Returns the product details</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var result = await Sender.Send(new GetProductByIdQuery(id));
            return HandleResult(result);
        }

        /// <summary> Get all products </summary>
        /// <remarks> Retrieves a paginated list of active products based on optional filters. </remarks>
        /// <response code="200">Returns a paginated result containing matching products</response>
        [RedisCache]
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await Sender.Send(new GetAllProductsQuery(queryParams));
            return Ok(products);
        }

        /// <summary> Get all types </summary>
        /// <remarks> Retrieves a list of all available product types. </remarks>
        /// <response code="200">Returns a collection of product types</response>
        [HttpGet("types")]
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var productTypes = await Sender.Send(new GetAllTypesQuery());
            return Ok(productTypes);
        }

        /// <summary> Get all themes </summary>
        /// <remarks> Retrieves a list of all available product themes. </remarks>
        /// <response code="200">Returns a collection of product themes</response>
        [HttpGet("themes")]
        public async Task<ActionResult<IEnumerable<ThemeDTO>>> GetAllThemes()
        {
            var productThemes = await Sender.Send(new GetAllThemesQuery());
            return Ok(productThemes);
        }

        /// <summary> Get all badges </summary>
        /// <remarks> Retrieves a list of all available product badges. </remarks>
        /// <response code="200">Returns a collection of product badges</response>
        [HttpGet("badges")]
        public async Task<ActionResult<IEnumerable<BadgeDTO>>> GetAllBadges()
        {
            var badges = await Sender.Send(new GetAllBadgesQuery());
            return Ok(badges);
        }
    }
}
