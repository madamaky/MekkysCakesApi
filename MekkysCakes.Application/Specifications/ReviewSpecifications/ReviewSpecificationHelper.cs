using System.Linq.Expressions;
using MekkysCakes.Application.Features.Reviews.Queries.GetAllReviews;
using MekkysCakes.Domain.Entities.ReviewModule;

namespace MekkysCakes.Application.Specifications.ReviewSpecifications
{
    public static class ReviewSpecificationHelper
    {
        public static Expression<Func<ProductReview, bool>> GetReviewCriteria(ReviewQueryParams queryParams)
            => r => (!queryParams.ProductId.HasValue || r.ProductId == queryParams.ProductId.Value)
                && (!queryParams.IsApproved.HasValue || r.IsApproved == queryParams.IsApproved.Value);
    }
}
