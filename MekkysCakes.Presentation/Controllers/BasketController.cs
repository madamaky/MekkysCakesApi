using MediatR;
using MekkysCakes.Application.Features.Baskets.Commands.CreateOrUpdateBasket;
using MekkysCakes.Application.Features.Baskets.Commands.DeleteBasket;
using MekkysCakes.Application.Features.Baskets.Queries.GetBasket;
using MekkysCakes.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        /// <returns>The contents of the requested shopping basket.</returns>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string basketId)
        {
            var result = await _sender.Send(new GetBasketQuery(basketId));
            return HandleResult(result);
        }

        /// <summary>
        /// Creates or updates a shopping basket for the currently authenticated user.
        /// </summary>
        /// <param name="basketDto">The updated basket data.</param>
        /// <returns>The fully updated shopping basket.</returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basketDto)
        {
            var result = await _sender.Send(new CreateOrUpdateBasketCommand(GetEmailFromToken(), basketDto.Items));
            return HandleResult(result);
        }

        /// <summary>
        /// Deletes a user's shopping basket. Only accessible by administrators.
        /// </summary>
        /// <param name="basketId">The identifier of the basket to delete.</param>
        /// <returns>True if the basket was successfully deleted; otherwise, false.</returns>
        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var result = await _sender.Send(new DeleteBasketCommand(basketId));
            return HandleResult(result);
        }
    }
}
