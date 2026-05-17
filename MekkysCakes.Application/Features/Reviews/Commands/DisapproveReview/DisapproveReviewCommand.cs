using MediatR;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.DisapproveReview
{
    public record DisapproveReviewCommand(int ReviewId) : IRequest<Result<bool>>;
}
