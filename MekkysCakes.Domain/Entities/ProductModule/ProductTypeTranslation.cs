namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class ProductTypeTranslation : BaseEntity<int>, ITranslation
    {
        public string Language { get; set; } = default!;
        public string Name { get; set; } = default!;

        public int ProductTypeId { get; set; }
        public ProductType ProductType { get; set; } = default!;
    }
}