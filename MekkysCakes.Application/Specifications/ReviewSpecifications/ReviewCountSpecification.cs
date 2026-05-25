using MekkysCakes.Application.Features.Reviews.Queries.GetAllReviews;
using MekkysCakes.Domain.Entities.ReviewModule;

namespace MekkysCakes.Application.Specifications.ReviewSpecifications
{
    public class ReviewCountSpecification : BaseSpecification<ProductReview, int>
    {
        public ReviewCountSpecification(ReviewQueryParams queryParams)
            : base(ReviewSpecificationHelper.GetReviewCriteria(queryParams)) { }

        public ReviewCountSpecification(int productId)
            : base(r => r.ProductId == productId && r.IsApproved) { }
    }
}
