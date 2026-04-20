using MediatR;
using MekkysCakes.Application.Features.Wishlists.Commands.AddItemToWishlist;
using MekkysCakes.Application.Features.Wishlists.Commands.RemoveItemFromWishlist;
using MekkysCakes.Application.Features.Wishlists.Queries.GetWishlist;
using MekkysCakes.Shared.DTOs.WishlistDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    /// <summary>
    /// Manages the user's wishlist of favorite products.
    /// </summary>
    [Authorize]
    public class WishlistController : ApiBaseController
    {
        private readonly ISender _sender;

        public WishlistController(ISender sender)
        {
            _sender = sender;
        }

        /// <summary>
        /// Retrieves all products currently in the authenticated user's wishlist.
        /// </summary>
        /// <returns>A collection of wishlist items.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WishlistItemDTO>>> GetWishlist()
        {
            var result = await _sender.Send(new GetWishlistQuery(GetEmailFromToken()));
            return HandleResult(result);
        }

        /// <summary>
        /// Adds a specified product to the user's wishlist.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to add.</param>
        /// <returns>True if the product was successfully added; otherwise, false.</returns>
        [HttpPost("{productId}")]
        public async Task<ActionResult<bool>> AddToWishlist(int productId)
        {
            var result = await _sender.Send(new AddItemToWishlistCommand(GetEmailFromToken(), productId));
            return HandleResult(result);
        }

        /// <summary>
        /// Removes a specified product from the user's wishlist.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        /// <returns>True if the product was successfully removed; otherwise, false.</returns>
        [HttpDelete("{productId}")]
        public async Task<ActionResult<bool>> RemoveFromWishlist(int productId)
        {
            var result = await _sender.Send(new RemoveItemFromWishlistCommand(GetEmailFromToken(), productId));
            return HandleResult(result);
        }
    }
}
