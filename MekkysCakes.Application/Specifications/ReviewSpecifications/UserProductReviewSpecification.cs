using MekkysCakes.Domain.Entities.ReviewModule;

namespace MekkysCakes.Application.Specifications.ReviewSpecifications
{
    public class UserProductReviewSpecification : BaseSpecification<ProductReview, int>
    {
        public UserProductReviewSpecification(int productId, string userId) : base(r => r.ProductId == productId && r.UserId == userId) { }
    }
}
