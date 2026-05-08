using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Application.Specifications.ProductSpecifications
{
    public class ThemeWithTranslationsSpecification : BaseSpecification<ProductTheme, int>
    {
        public ThemeWithTranslationsSpecification() : base(t => true)
        {
            AddInclude(t => t.Translations);
        }
    }
}
