using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.IdentityModule;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using Microsoft.AspNetCore.Identity;

namespace MekkysCakes.Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            // Get the current user
            var userId = _currentUserService.UserId;
            if (userId is null)
                return Error.Unauthorized("User.Unauthorized", "The User Was Not Found");

            // Find the review
            var review = await _unitOfWork.GetRepository<ProductReview, int>().GetByIdAsync(request.ReviewId);
            if (review is null)
                return Error.NotFound("Review.NotFound", $"The Review With Id {request.ReviewId} Was Not Found");

            // Verify ownership
            if (review.UserId != userId)
                return Error.Forbidden("Review.Forbidden", "You Do Not Have Permission To Delete This Review");

            // Recalculate the product's average rating before deletion
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(review.ProductId);
            if (product is not null && product.TotalReviews > 0)
            {
                if (product.TotalReviews == 1)
                {
                    product.AverageRating = 0;
                    product.TotalReviews = 0;
                }
                else
                {
                    product.AverageRating = ((product.AverageRating * product.TotalReviews) - review.Rating) / (product.TotalReviews - 1);
                    product.TotalReviews -= 1;
                }
                _unitOfWork.GetRepository<Product, int>().Update(product);
            }
            
            // Delete the review
            _unitOfWork.GetRepository<ProductReview, int>().Delete(review);

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
