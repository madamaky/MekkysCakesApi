using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.DTOs.WishlistDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    [Authorize]
    public class WishlistController : ApiBaseController
    {
        private readonly IWishlistService _wishlistService;
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItemDTO>>> GetWishlist()
        {
            var result = await _wishlistService.GetWishlistAsync(GetEmailFromToken());
            return HandleResult(result);
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult<bool>> AddToWishlist(int productId)
        {
            var result = await _wishlistService.AddItemToWishlistAsync(GetEmailFromToken(), productId);
            return HandleResult(result);
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> RemoveFromWishlist(int productId)
        {
            var result = await _wishlistService.RemoveItemFromWishlistAsync(GetEmailFromToken(), productId);
            return HandleResult(result);
        }
    }
}
