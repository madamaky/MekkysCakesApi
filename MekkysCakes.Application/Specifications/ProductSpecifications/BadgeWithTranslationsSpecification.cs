using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Specifications.ProductSpecifications
{
    public class BadgeWithTranslationsSpecification : BaseSpecification<Badge, int>
    {
        public BadgeWithTranslationsSpecification() : base(b => true)
        {
            AddInclude(b => b.Translations);
        }
    }
}