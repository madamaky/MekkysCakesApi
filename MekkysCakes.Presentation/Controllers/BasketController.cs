using MediatR;
using MekkysCakes.Application.Features.Baskets.Commands.CreateOrUpdateBasket;
using MekkysCakes.Application.Features.Baskets.Commands.DeleteBasket;
using MekkysCakes.Application.Features.Baskets.Queries.GetBasket;
using MekkysCakes.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace MekkysCakes.Presentation.Controllers
{
    /// <summary>
    /// Handles operations for user shopping baskets.
    /// </summary>
    public class BasketController : ApiBaseController
    {
        private readonly ISender _sender;

        public BasketController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Retrieves a shopping basket by its identifier.
        /// </summary>
        /// <param name="basketId">The unique identifier of the basket.</param>
        /// <remarks>
        /// Gets the current state of a user's shopping basket including all items and totals.
        /// 
        /// Sample request:
        ///
        ///     GET /api/basket?basketId=basket-123
        ///
        /// </remarks>
        /// <response code="200">Returns the requested shopping basket</response>
        /// <response code="401">Unauthorized access</response>
        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(BasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BasketDTO>> GetBasket(string basketId)
        {
            var result = await _sender.Send(new GetBasketQuery(basketId));
            return HandleResult(result);
        }

        /// <summary>
        /// Creates or updates a shopping basket for the currently authenticated user.
        /// </summary>
        /// <param name="basketDto">The updated basket data.</param>
        /// <remarks>
        /// This endpoint will replace the contents of the existing basket or create a new one if it doesn't exist.
        /// 
        /// Sample request:
        ///
        ///     POST /api/basket
        ///     {
        ///        "id": "basket-123",
        ///        "items": [
        ///           { "productId": 1, "productName": "Cake", "price": 10.99, "quantity": 1 }
        ///        ],
        ///        "shippingPrice": 5.00
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns the fully updated shopping basket</response>
        /// <response code="400">Invalid basket data supplied</response>
        /// <response code="401">Unauthorized access</response>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(BasketDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basketDto)
        {
            var result = await _sender.Send(new CreateOrUpdateBasketCommand(GetEmailFromToken(), basketDto.Items));
            return HandleResult(result);
        }

        /// <summary>
        /// Deletes a user's shopping basket. Only accessible by administrators.
        /// </summary>
        /// <param name="basketId">The identifier of the basket to delete.</param>
        /// <remarks>
        /// Removes the basket from the system entirely.
        /// 
        /// Sample request:
        ///
        ///     DELETE /api/basket/basket-123
        ///
        /// </remarks>
        /// <response code="200">Basket was successfully deleted</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="403">Forbidden access (not an admin)</response>
        /// <response code="404">Basket not found</response>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{basketId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var result = await _sender.Send(new DeleteBasketCommand(basketId));
            return HandleResult(result);
        }
    }
}
