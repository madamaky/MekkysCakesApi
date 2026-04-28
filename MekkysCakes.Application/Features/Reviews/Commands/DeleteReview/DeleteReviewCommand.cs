using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand
    (
        int ReviewId,
        string UserEmail
    ) : IRequest<Result<bool>>;
}
