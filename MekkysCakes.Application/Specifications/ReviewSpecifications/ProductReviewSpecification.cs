using MekkysCakes.Domain.Entities.ReviewModule;

namespace MekkysCakes.Application.Specifications.ReviewSpecifications
{
    public class ProductReviewSpecification : BaseSpecification<ProductReview, int>
    {
        public ProductReviewSpecification(int productId, string sort, int skip, int take)
            : base(r => r.ProductId == productId && r.IsApproved)
        {
            AddInclude(r => r.User);
            ApplyPagination(skip, take);

            // Use enums!
            switch (sort.ToLower())
            {
                case "oldest":
                    AddOrderBy(r => r.CreatedAt);
                    break;
                case "highest":
                    AddOrderByDescending(r => r.Rating);
                    break;
                case "lowest":
                    AddOrderBy(r => r.Rating);
                    break;
                default: // "newest"
                    AddOrderByDescending(r => r.CreatedAt);
                    break;
            }
        }
    }
}
