using MekkysCakes.Application.Features.Reviews.Commands.ApproveReview;
using MekkysCakes.Application.Features.Reviews.Commands.DisapproveReview;
using MekkysCakes.Application.Features.Reviews.Queries.GetAllReviews;
using MekkysCakes.Domain;
using MekkysCakes.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MekkysCakes.Presentation.Controllers.AdminControllers
{
    [Authorize(Policy = AuthorizationPolicies.AdminDashboard)]
    [Route("api/admin/reviews")]
    public class AdminReviewController : ApiBaseController
    {
        /// <summary> Disapprove review </summary>
        /// <remarks> Transitions a review from publicly visible to disapproved. </remarks>
        /// <response code="200">Returns true if the review was disapproved</response>
        [HttpPatch("{reviewId}/disapprove")]
        public async Task<ActionResult<bool>> DisapproveReview(int reviewId)
        {
            var result = await Sender.Send(new DisapproveReviewCommand(reviewId));
            return HandleResult(result);
        }


        /// <summary> Approve review </summary>
        /// <remarks> Transitions a review from disapproved to publicly visible. </remarks>
        /// <response code="200">Returns true if the review was approved</response>
        [HttpPatch("{reviewId}/approve")]
        public async Task<ActionResult<bool>> ApproveReview(int reviewId)
        {
            var result = await Sender.Send(new ApproveReviewCommand(reviewId));
            return HandleResult(result);
        }


        /// <summary> Get product reviews </summary>
        /// <remarks> Get paginated reviews for a specific product. Only returns approved reviews for public view. </remarks>
        /// <response code="200">Returns paginated reviews</response>
        [HttpGet("getAllProductsReviews")]
        public async Task<ActionResult<PaginatedResult<ReviewDTO>>> GetAllProductsReviews([FromQuery] ReviewQueryParams queryParams)
        {
            var result = await Sender.Send(new GetAllReviewsQuery(queryParams));
            return Ok(result);
        }
    }
}
