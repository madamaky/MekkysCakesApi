using MekkysCakes.Application.Features.Wishlists.Commands.AddItemToWishlist;
using MekkysCakes.Application.Features.Wishlists.Commands.RemoveItemFromWishlist;
using MekkysCakes.Application.Features.Wishlists.Queries.GetWishlist;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    [Authorize]
    public class WishlistController : ApiBaseController
    {
        /// <summary> Get wishlist </summary>
        /// <remarks> Retrieves all products currently in the authenticated user's wishlist. </remarks>
        /// <response code="200">Returns a collection of wishlist items</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItemDTO>>> GetWishlist()
        {
            var result = await Sender.Send(new GetWishlistQuery());
            return HandleResult(result);
        }

        /// <summary> Add to wishlist </summary>
        /// <remarks> Adds a specified product to the user's wishlist. </remarks>
        /// <response code="200">Returns true if the product was successfully added</response>
        [HttpPost("{productId}")]
        public async Task<ActionResult<bool>> AddToWishlist(int productId)
        {
            var result = await Sender.Send(new AddItemToWishlistCommand(productId));
            return HandleResult(result);
        }

        /// <summary> Remove from wishlist </summary>
        /// <remarks> Removes a specified product from the user's wishlist. </remarks>
        /// <response code="200">Returns true if the product was successfully removed</response>
        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> RemoveFromWishlist(int productId)
        {
            var result = await Sender.Send(new RemoveItemFromWishlistCommand(productId));
            return HandleResult(result);
        }
    }
}
