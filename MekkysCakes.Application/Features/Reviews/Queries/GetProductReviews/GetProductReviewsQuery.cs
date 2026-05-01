using MediatR;
using MekkysCakes.Shared;

namespace MekkysCakes.Application.Features.Reviews.Queries.GetProductReviews
{
    public record GetProductReviewsQuery
    (
        int ProductId,
        int PageIndex = 1,
        int PageSize = 10,
        string Sort = "newest"  // "newest", "oldest", "highest", "lowest"
    ) : IRequest<PaginatedResult<ReviewDTO>>;

    public record ReviewDTO(
        int Id,
        int Rating,
        string? Title,
        string? Comment,
        string UserDisplayName,
        DateTime CreatedAt,
        DateTime? UpdatedAt);
}
