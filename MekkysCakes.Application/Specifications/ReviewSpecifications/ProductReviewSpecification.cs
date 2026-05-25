using MekkysCakes.Application.Features.Reviews.Queries.GetAllReviews;
using MekkysCakes.Domain.Entities.ReviewModule;
using MekkysCakes.Shared.DTOs.ReviewDTOs;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Application.Specifications.ReviewSpecifications
{
    public class ProductReviewSpecification : BaseSpecification<ProductReview, int>
    {
        public ProductReviewSpecification(int productId, string sort, int skip, int take)
            : base(r => r.ProductId == productId && r.IsApproved)
        {
            AddInclude(r => r.User);
            ApplyPagination(skip, take);

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

        public ProductReviewSpecification(ReviewQueryParams queryParams)
            : base(ReviewSpecificationHelper.GetReviewCriteria(queryParams))
        {
            AddInclude(r => r.User);
            AddThenInclude(r => r
                .Include(r => r.Product)
                .ThenInclude(p => p.Translations)
            );

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);

            switch (queryParams.Sort)
            {
                case ReviewSortingOptions.Newest:
                    AddOrderByDescending(r => r.CreatedAt);
                    break;
                case ReviewSortingOptions.Oldest:
                    AddOrderBy(r => r.CreatedAt);
                    break;
                default:
                    AddOrderByDescending(r => r.CreatedAt);
                    break;
            }
        }
    }
}
