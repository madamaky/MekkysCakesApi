using MediatR;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.IdentityModule;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;
using Microsoft.AspNetCore.Identity;

namespace MekkysCakes.Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
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
                return Error.Forbidden("Review.Forbidden", "You Do Not Have Permission To Update This Review");

            // Store old rating before updating
            var oldRating = review.Rating;

            // Update the review
            review.Rating = request.Rating;
            review.Title = request.Title;
            review.Comment = request.Comment;
            review.UpdatedAt = DateTime.UtcNow;
            review.IsApproved = false; // Reset approval

            _unitOfWork.GetRepository<ProductReview, int>().Update(review);

            // Recalculate product's average rating
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(review.ProductId);
            if (product is not null && product.TotalReviews > 0)
            {
                product.AverageRating = ((product.AverageRating * product.TotalReviews) - oldRating + request.Rating) / product.TotalReviews;
                _unitOfWork.GetRepository<Product, int>().Update(product);
            }

            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
