using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.ApproveReview
{
    public class ApproveReviewCommandHandler : IRequestHandler<ApproveReviewCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApproveReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(ApproveReviewCommand request, CancellationToken cancellationToken)
        {
            // Find the review
            var review = await _unitOfWork.GetRepository<ProductReview, int>().GetByIdAsync(request.ReviewId);
            if (review is null)
                return Error.NotFound("Review.NotFound", $"The Review With Id {request.ReviewId} Was Not Found");

            // Check if already approved
            if (review.IsApproved)
                return Error.Validation("Review.Validation", $"The Review With Id {request.ReviewId} Is Already Approved");

            // Approve the review
            review.IsApproved = true;
            _unitOfWork.GetRepository<ProductReview, int>().Update(review);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
