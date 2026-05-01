using MediatR;
using MekkysCakes.Application.Specifications.ReviewSpecifications;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared.CommonResult;

namespace MekkysCakes.Application.Features.Reviews.Queries.GetReviewSummary
{
    public class GetReviewSummaryQueryHandler : IRequestHandler<GetReviewSummaryQuery, Result<ReviewSummaryDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetReviewSummaryQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ReviewSummaryDTO>> Handle(GetReviewSummaryQuery request, CancellationToken cancellationToken)
        {
            // Verify product exists
            var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(request.ProductId);
            if (product is null)
                return Error.NotFound("Product.NotFound", $"The Product With Id {request.ProductId} Was Not Found");

            // Get all approved reviews for this product
            var reviewSpec = new ReviewCountSpecification(request.ProductId);
            var allApprovedReviews = await _unitOfWork.GetRepository<ProductReview, int>().GetAllAsync(reviewSpec);
            var reviewsList = allApprovedReviews.ToList();

            // Build the rating distribution dictionary
            var distribution = new Dictionary<int, int>
            {
                { 1, 0 }, { 2, 0 }, { 3, 0 }, { 4, 0 }, { 5, 0 }
            };
            foreach (var review in reviewsList)
                if (distribution.ContainsKey(review.Rating))
                    distribution[review.Rating]++;

            // Build the summary DTO
            return new ReviewSummaryDTO(product.AverageRating, product.TotalReviews, distribution);
        }
    }
}
