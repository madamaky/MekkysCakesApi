using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Specifications.ProductSpecifications
{
    public class ProductWithBadgesSpecification : BaseSpecification<Product, int>
    {
        public ProductWithBadgesSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductBadges);
        }
    }
}
