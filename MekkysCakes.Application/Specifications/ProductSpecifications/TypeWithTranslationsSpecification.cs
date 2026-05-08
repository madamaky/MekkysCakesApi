using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Specifications.ProductSpecifications
{
    public class TypeWithTranslationsSpecification : BaseSpecification<ProductType, int>
    {
        public TypeWithTranslationsSpecification() : base(t => true)
        {
            AddInclude(t => t.Translations);
        }
    }
}
