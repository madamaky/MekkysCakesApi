using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.CreateReview
{
    public record CreateReviewCommand
    (
        int ProductId, 
        int Rating, 
        string? Title, 
        string? Comment
    ) : IRequest<Result<ReviewCreatedDTO>>;

    public record ReviewCreatedDTO
    (
        int ReviewId, 
        decimal NewAverageRating, 
        int NewTotalReviews
    );
}
