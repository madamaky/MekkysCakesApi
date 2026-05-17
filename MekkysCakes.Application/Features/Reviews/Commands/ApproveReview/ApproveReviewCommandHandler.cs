using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
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
            // Check if the review exists
            var review = await _unitOfWork.GetRepository<ProductReview, int>().GetByIdAsync(request.ReviewId);
            if (review is null)
                return Error.NotFound("Review.NotFound", $"The Review With Id {request.ReviewId} Was Not Found");

            // Check if already approved
            if (review.IsApproved)
                return Error.Validation("Review.Validation", $"The Review With Id {request.ReviewId} Is Already Approved");

            // Check if product exists
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(review.ProductId);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {review.ProductId} Was Not Found");

            // Recalculate the product's average rating and total reviews
            product.IncludeReviewInRatings(review.Rating);
            _unitOfWork.GetRepository<Product, int>().Update(product);

            // Approve the review
            review.IsApproved = true;
            _unitOfWork.GetRepository<ProductReview, int>().Update(review);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
