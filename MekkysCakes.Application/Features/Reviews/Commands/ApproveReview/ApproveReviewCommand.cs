using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.ApproveReview
{
    public record ApproveReviewCommand(int ReviewId) : IRequest<Result<bool>>;
}
