using MekkysCakes.Application.Features.Reviews.Commands.CreateReview;
using MekkysCakes.Application.Features.Reviews.Commands.DeleteReview;
using MekkysCakes.Application.Features.Reviews.Commands.UpdateReview;
using MekkysCakes.Application.Features.Reviews.Queries.GetProductReviews;
using MekkysCakes.Application.Features.Reviews.Queries.GetReviewSummary;
using MekkysCakes.Shared;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers
{
    public class ReviewController : ApiBaseController
    {
        /// <summary> Get product reviews </summary>
        /// <remarks> Get paginated reviews for a specific product. Only returns approved reviews for public view. </remarks>
        /// <response code="200">Returns paginated reviews</response>
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<PaginatedResult<ReviewDTO>>> GetProductReviews(int productId, [FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, [FromQuery] string sort = "newest")
        {
            var result = await Sender.Send(
                new GetProductReviewsQuery(productId, pageIndex, pageSize, sort));
            return Ok(result);
        }
        /// <summary> Get review summary </summary>
        /// <remarks> Get review statistics/summary for a product (average, count, distribution). </remarks>
        /// <response code="200">Returns review summary</response>
        [HttpGet("product/{productId}/summary")]
        public async Task<ActionResult<ReviewSummaryDTO>> GetReviewSummary(int productId)
        {
            var result = await Sender.Send(new GetReviewSummaryQuery(productId));
            return HandleResult(result);
        }
        /// <summary> Create review </summary>
        /// <remarks> Submit a new review for a product. Requires authentication. The review will need admin approval before it appears publicly. </remarks>
        /// <response code="200">Returns the created review</response>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ReviewCreatedDTO>> CreateReview([FromBody] CreateReviewCommand request)
        {
            var result = await Sender.Send(request);
            return HandleResult(result);
        }
        /// <summary> Update review </summary>
        /// <remarks> Update your own review. </remarks>
        /// <response code="200">Returns true if updated successfully</response>
        [Authorize]
        [HttpPut("{reviewId}")]
        public async Task<ActionResult<bool>> UpdateReview(int reviewId, [FromBody] UpdateReviewCommand request)
        {
            var result = await Sender.Send(request);
            return HandleResult(result);
        }
        /// <summary> Delete review </summary>
        /// <remarks> Delete your own review. </remarks>
        /// <response code="204">Review successfully deleted</response>
        [Authorize]
        [HttpDelete("{reviewId}")]
        public async Task<ActionResult<bool>> DeleteReview(int reviewId)
        {
            var result = await Sender.Send(new DeleteReviewCommand(reviewId));
            return HandleResult(result);
        }
    }
}