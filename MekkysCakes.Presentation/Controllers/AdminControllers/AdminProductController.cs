using MekkysCakes.Application.Features.Products.Commands.CreateProduct;
using MekkysCakes.Application.Features.Products.Commands.DeleteProduct;
using MekkysCakes.Application.Features.Products.Commands.UpdateProduct;
using MekkysCakes.Application.Features.Products.Queries.GetAllProducts;
using MekkysCakes.Domain;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers.AdminControllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminDashboard)]
    [Route("api/admin/products")]
    public class AdminProductController : ApiBaseController
    {
        /// <summary> Get all products </summary>
        /// <remarks> Retrieves a paginated list of all products, including inactive ones. </remarks>
        /// <response code="200">Returns a paginated result containing all matching products</response>
        [HttpGet]
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await Sender.Send(new GetAllProductsQuery(queryParams));
            return Ok(products);
        }

        /// <summary> Create product </summary>
        /// <remarks> Creates a new product in the catalog. </remarks>
        /// <response code="200">Returns true if the product was successfully created</response>
        [HttpPost]
        public async Task<ActionResult<bool>> CreateProduct(CreateProductCommand command)
        {
            var result = await Sender.Send(command);
            return HandleResult(result);
        }

        /// <summary> Update product </summary>
        /// <remarks> Updates an existing product's details. </remarks>
        /// <response code="200">Returns true if the product was successfully updated</response>
        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateProduct([FromRoute] int id, [FromBody] UpdateProductCommand command)
        {
            // We need to ensure the route id matches the command id
            // Create a new command with the route id (records are immutable, so we use "with")
            var commandWithId = command with { Id = id };
            var result = await Sender.Send(commandWithId);
            return HandleResult(result);
        }

        /// <summary> Delete product </summary>
        /// <remarks> Soft-deletes a product from the catalog. </remarks>
        /// <response code="200">Returns true if the product was successfully removed</response>
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            var result = await Sender.Send(new DeleteProductCommand(id));
            return HandleResult(result);
        }
    }
}
