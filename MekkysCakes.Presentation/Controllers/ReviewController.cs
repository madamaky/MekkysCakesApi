using MediatR;
using MekkysCakes.Application.Features.Reviews.Commands.ApproveReview;
using MekkysCakes.Application.Features.Reviews.Commands.CreateReview;
using MekkysCakes.Application.Features.Reviews.Commands.DeleteReview;
using MekkysCakes.Application.Features.Reviews.Commands.UpdateReview;
using MekkysCakes.Application.Features.Reviews.Queries.GetProductReviews;
using MekkysCakes.Application.Features.Reviews.Queries.GetReviewSummary;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ReviewDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    /// <summary>
    /// Manages product reviews — submission, retrieval, and moderation.
    /// </summary>
    public class ReviewController : ApiBaseController
    {
        private readonly ISender _sender;
        public ReviewController(ISender sender) => _sender = sender;
        /// <summary>
        /// Get paginated reviews for a specific product.
        /// Only returns approved reviews for public view.
        /// </summary>
        [HttpGet("product/{productId}")]
        [ProducesResponseType(typeof(PaginatedResult<ReviewDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PaginatedResult<ReviewDTO>>> GetProductReviews(
            int productId,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sort = "newest")
        {
            var result = await _sender.Send(
                new GetProductReviewsQuery(productId, pageIndex, pageSize, sort));
            return Ok(result);
        }
        /// <summary>
        /// Get review statistics/summary for a product (average, count, distribution).
        /// </summary>
        [HttpGet("product/{productId}/summary")]
        [ProducesResponseType(typeof(ReviewSummaryDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<ReviewSummaryDTO>> GetReviewSummary(int productId)
        {
            var result = await _sender.Send(new GetReviewSummaryQuery(productId));
            return HandleResult(result);
        }
        /// <summary>
        /// Submit a new review for a product. Requires authentication.
        /// The review will need admin approval before it appears publicly.
        /// </summary>
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ReviewCreatedDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ReviewCreatedDTO>> CreateReview(
            [FromBody] CreateReviewRequest request)
        {
            var userEmail = GetEmailFromToken();
            // You'll need to resolve userId from email via UserManager
            // Or add a GetUserIdFromToken() method to ApiBaseController

            var command = new CreateReviewCommand(
                request.ProductId,
                request.Rating,
                request.Title,
                request.Comment,
                userEmail // Will be resolved to UserId in handler
            );
            var result = await _sender.Send(command);
            return HandleResult(result);
        }
        /// <summary>
        /// Update your own review.
        /// </summary>
        [Authorize]
        [HttpPut("{reviewId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> UpdateReview(
            int reviewId, [FromBody] UpdateReviewRequest request)
        {
            var userEmail = GetEmailFromToken();
            var command = new UpdateReviewCommand(
                reviewId, request.Rating, request.Title,
                request.Comment, userEmail);
            var result = await _sender.Send(command);
            return HandleResult(result);
        }
        /// <summary>
        /// Delete your own review.
        /// </summary>
        [Authorize]
        [HttpDelete("{reviewId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> DeleteReview(int reviewId)
        {
            var userEmail = GetEmailFromToken();
            var result = await _sender.Send(
                new DeleteReviewCommand(reviewId, userEmail));
            return HandleResult(result);
        }
        /// <summary>
        /// Admin: Approve a review to make it publicly visible.
        /// </summary>
        [Authorize(Roles = "SuperAdmin")]
        [HttpPatch("{reviewId}/approve")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ApproveReview(int reviewId)
        {
            var result = await _sender.Send(new ApproveReviewCommand(reviewId));
            return HandleResult(result);
        }
    }
    // Request models (what the frontend POST body looks like)
    // These are separate from Commands because Commands include UserId from JWT
    public record CreateReviewRequest(
        int ProductId, int Rating, string? Title, string? Comment);
    public record UpdateReviewRequest(
        int Rating, string? Title, string? Comment);
}
