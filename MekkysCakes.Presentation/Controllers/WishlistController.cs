using MediatR;
using MekkysCakes.Application.Features.Wishlists.Commands.AddItemToWishlist;
using MekkysCakes.Application.Features.Wishlists.Commands.RemoveItemFromWishlist;
using MekkysCakes.Application.Features.Wishlists.Queries.GetWishlist;
using MekkysCakes.Shared.DTOs.WishlistDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

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
        /// <remarks>
        /// Returns a lightweight list identifying which products the user has favorited.
        /// 
        /// Sample request:
        ///
        ///     GET /api/wishlist
        ///
        /// </remarks>
        /// <response code="200">Returns a collection of wishlist items</response>
        /// <response code="401">Unauthorized access</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<WishlistItemDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<WishlistItemDTO>>> GetWishlist()
        {
            var result = await _sender.Send(new GetWishlistQuery(GetEmailFromToken()));
            return HandleResult(result);
        }

        /// <summary>
        /// Adds a specified product to the user's wishlist.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to add.</param>
        /// <remarks>
        /// Marks a product as favorited without adding it to the shopping basket.
        /// 
        /// Sample request:
        ///
        ///     POST /api/wishlist/42
        ///
        /// </remarks>
        /// <response code="200">Returns true if the product was successfully added</response>
        /// <response code="400">Product already in wishlist or invalid ID</response>
        /// <response code="401">Unauthorized access</response>
        [HttpPost("{productId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> AddToWishlist(int productId)
        {
            var result = await _sender.Send(new AddItemToWishlistCommand(GetEmailFromToken(), productId));
            return HandleResult(result);
        }

        /// <summary>
        /// Removes a specified product from the user's wishlist.
        /// </summary>
        /// <param name="productId">The unique identifier of the product to remove.</param>
        /// <remarks>
        /// Un-favorites a product. Safe to call even if the product wasn't in the wishlist.
        /// 
        /// Sample request:
        ///
        ///     DELETE /api/wishlist/42
        ///
        /// </remarks>
        /// <response code="200">Returns true if the product was successfully removed</response>
        /// <response code="401">Unauthorized access</response>
        /// <response code="404">Product ID not found in current wishlist</response>
        [HttpDelete("{productId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> RemoveFromWishlist(int productId)
        {
            var result = await _sender.Send(new RemoveItemFromWishlistCommand(GetEmailFromToken(), productId));
            return HandleResult(result);
        }
    }
}
