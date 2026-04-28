using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.UpdateReview
{
    public record UpdateReviewCommand
    (
        int ReviewId,
        int Rating,
        string? Title,
        string? Comment,
        string UserEmail
    ) : IRequest<Result<bool>>;
}
