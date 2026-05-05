namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class ProductBadge
    {
        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public int BadgeId { get; set; }
        public Badge Badge { get; set; } = default!;
    }
}
