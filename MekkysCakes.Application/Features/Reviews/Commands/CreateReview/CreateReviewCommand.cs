using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.CreateReview
{
    public record CreateReviewCommand
    (
        int ProductId,
        int Rating,      // 1-5
        string? Title,
        string? Comment,
        string UserId    // Set by controller from JWT, not from request body
    ) : IRequest<Result<ReviewCreatedDTO>>;

    public record ReviewCreatedDTO(int ReviewId, decimal NewAverageRating, int NewTotalReviews);
}
