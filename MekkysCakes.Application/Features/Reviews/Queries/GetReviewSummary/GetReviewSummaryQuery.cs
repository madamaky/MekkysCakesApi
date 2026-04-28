using MediatR;
using MekkysCakes.Shared.CommonResult;
using MekkysCakes.Shared.DTOs.ReviewDTOs;

namespace MekkysCakes.Application.Features.Reviews.Queries.GetReviewSummary
{
    public record GetReviewSummaryQuery(int ProductId) : IRequest<Result<ReviewSummaryDTO>>;
}
