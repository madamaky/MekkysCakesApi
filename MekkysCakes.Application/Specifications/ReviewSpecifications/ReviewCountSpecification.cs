using MekkysCakes.Domain.Entities.ReviewModule;

namespace MekkysCakes.Application.Specifications.ReviewSpecifications
{
    public class ReviewCountSpecification : BaseSpecification<ProductReview, int>
    {
        public ReviewCountSpecification(int productId) : base(r => r.ProductId == productId && r.IsApproved) { }
    }
}
