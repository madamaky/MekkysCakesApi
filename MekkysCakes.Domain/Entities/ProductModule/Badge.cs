namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class Badge : BaseEntity<int>
    {
        public string Name { get; set; } = default!;

        // Nav Property
        public ICollection<ProductBadge> ProductBadges { get; set; } = [];
    }
}
