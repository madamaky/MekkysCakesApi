namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class ProductTranslation : BaseEntity<int>, ITranslation
    {
        public string Language { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;
    }
}