using MediatR;
using MekkysCakes.Shared;
using MekkysCakes.Shared.DTOs.ReviewDTOs;

namespace MekkysCakes.Application.Features.Reviews.Queries.GetProductReviews
{
    public record GetProductReviewsQuery
    (
        int ProductId,
        int PageIndex = 1,
        int PageSize = 10,
        string Sort = "newest"  // "newest", "oldest", "highest", "lowest"
    ) : IRequest<PaginatedResult<ReviewDTO>>;
}
