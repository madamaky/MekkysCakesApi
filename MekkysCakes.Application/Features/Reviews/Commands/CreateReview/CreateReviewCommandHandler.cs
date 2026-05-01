using MediatR;
using MekkysCakes.Application.Specifications.ReviewSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Services.Abstraction;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Result<ReviewCreatedDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateReviewCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ReviewCreatedDTO>> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            // Check if the product exists
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(request.ProductId);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {request.ProductId} Was Not Found");

            // Check if the user is authenticated
            var userId = _currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
                return Error.Unauthorized("User.Unauthorized", "You Must Be Logged In To Review A Product");

            // Check if the user has already reviewed this product
            var spec = new UserProductReviewSpecification(request.ProductId, userId);
            var existingReview = await _unitOfWork.GetRepository<ProductReview, int>().GetAllAsync(spec);
            if (existingReview.Any())
                return Error.Validation("Review.Validation", "User Has Already Reviewed This Product");

            // Create the review
            var review = new ProductReview
            {
                ProductId = request.ProductId,
                UserId = userId,
                Rating = request.Rating,
                Title = request.Title,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow,
                IsApproved = false
            };
            await _unitOfWork.GetRepository<ProductReview, int>().AddAsync(review);

            // Recalculate product rating aggregates (denormalization for performance)
            product.TotalReviews += 1;
            product.AverageRating = ((product.AverageRating * (product.TotalReviews - 1)) + request.Rating) / product.TotalReviews;
            _unitOfWork.GetRepository<Product, int>().Update(product);

            await _unitOfWork.SaveChangesAsync();
            return new ReviewCreatedDTO(review.Id, product.AverageRating, product.TotalReviews);
        }
    }
}
