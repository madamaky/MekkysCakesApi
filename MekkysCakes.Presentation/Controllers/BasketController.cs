using MekkysCakes.Application.Features.Baskets.Commands.CreateOrUpdateBasket;
using MekkysCakes.Application.Features.Baskets.Commands.DeleteBasket;
using MekkysCakes.Application.Features.Baskets.Queries.GetBasket;
using MekkysCakes.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class BasketController : ApiBaseController
    {
        /// <summary> Get basket </summary>
        /// <remarks> Retrieves a shopping basket by its identifier. </remarks>
        /// <response code="200">Returns the requested shopping basket</response>
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket()
        {
            var result = await Sender.Send(new GetBasketQuery());
            return HandleResult(result);
        }

        /// <summary> Create or update basket </summary>
        /// <remarks> Creates or updates a shopping basket for the currently authenticated user. </remarks>
        /// <response code="200">Returns the fully updated shopping basket</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basketDto)
        {
            var result = await Sender.Send(new CreateOrUpdateBasketCommand(basketDto.Items));
            return HandleResult(result);
        }

        /// <summary> Delete basket </summary>
        /// <remarks> Deletes a user's shopping basket. </remarks>
        /// <response code="200">Basket was successfully deleted</response>
        [Authorize]
        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var result = await Sender.Send(new DeleteBasketCommand(basketId));
            return HandleResult(result);
        }
    }
}
