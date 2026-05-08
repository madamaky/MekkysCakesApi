using MekkysCakes.Domain.Entities.OrderModule;

namespace MekkysCakes.Application.Specifications.OrderSpecifications
{
    public class DeliveryMethodsWithTranslationsSpecification : BaseSpecification<DeliveryMethod, int>
    {
        public DeliveryMethodsWithTranslationsSpecification() : base(dm => true)
        {
            AddInclude(dm => dm.Translations);
        }
    }
}
