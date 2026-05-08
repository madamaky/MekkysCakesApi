namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class Badge : BaseEntity<int>, ITranslatableEntity<BadgeTranslation>
    {
        //public string Name { get; set; } = default!;

        // Nav Property
        public ICollection<ProductBadge> ProductBadges { get; set; } = [];
        public ICollection<BadgeTranslation> Translations { get; set; } = [];
    }
}
