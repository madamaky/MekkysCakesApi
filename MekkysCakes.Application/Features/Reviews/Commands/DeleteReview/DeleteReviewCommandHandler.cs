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
        private readonly IIdentityService _identityService;

        public DeleteReviewCommandHandler(IUnitOfWork unitOfWork, IIdentityService identityService)
        {
            _unitOfWork = unitOfWork;
            _identityService = identityService;
        }

        public async Task<Result<bool>> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            // Resolve user from email
            var user = await _identityService.FindByEmailAsync(request.UserEmail);
            if (user is null)
                return Error.Unauthorized("User.Unauthorized", "The User Was Not Found");

            // Find the review
            var review = await _unitOfWork.GetRepository<ProductReview, int>().GetByIdAsync(request.ReviewId);
            if (review is null)
                return Error.NotFound("Review.NotFound", $"The Review With Id {request.ReviewId} Was Not Found");

            // Verify ownership
            if (review.UserId != user.Id)
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
