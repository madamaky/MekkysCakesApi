using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Queries.GetReviewSummary
{
    public record GetReviewSummaryQuery(int ProductId) : IRequest<Result<ReviewSummaryDTO>>;

    public record ReviewSummaryDTO(decimal AverageRating, int TotalReviews, Dictionary<int, int> RatingDistribution);
}
