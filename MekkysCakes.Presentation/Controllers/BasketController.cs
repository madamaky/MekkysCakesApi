using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.DTOs.BasketDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class BasketController : ApiBaseController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<BasketDTO>> GetBasket(string basketId)
        {
            var basket = await _basketService.GetBasketAsync(basketId);
            return Ok(basket);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BasketDTO>> CreateOrUpdateBasket(BasketDTO basketDto)
        {
            var basket = await _basketService.CreateOrUpdateBasketAsync(GetEmailFromToken(), basketDto);
            return Ok(basket);
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpDelete("{basketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string basketId)
        {
            var result = await _basketService.DeleteBasketAsync(basketId);
            return Ok(result);
        }
    }
}
