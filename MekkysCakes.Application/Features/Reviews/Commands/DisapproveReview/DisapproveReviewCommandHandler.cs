using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.DisapproveReview
{
    public class DisapproveReviewCommandHandler : IRequestHandler<DisapproveReviewCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DisapproveReviewCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(DisapproveReviewCommand request, CancellationToken cancellationToken)
        {
            // Check if the review exists
            var review = await _unitOfWork.GetRepository<ProductReview, int>().GetByIdAsync(request.ReviewId);
            if (review is null)
                return Error.NotFound("Review.NotFound", $"The Review With Id {request.ReviewId} Was Not Found");

            // Check if already disapproved
            if (!review.IsApproved)
                return Error.Validation("Review.Validation", $"The Review With Id {request.ReviewId} Is Already Disapproved");

            // Check if product exists
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(review.ProductId);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {review.ProductId} Was Not Found");

            // Recalculate the product's average rating and total reviews
            product.ExcludeReviewFromRatings(review.Rating);
            _unitOfWork.GetRepository<Product, int>().Update(product);

            // Disapprove the review
            review.IsApproved = false;
            _unitOfWork.GetRepository<ProductReview, int>().Update(review);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
