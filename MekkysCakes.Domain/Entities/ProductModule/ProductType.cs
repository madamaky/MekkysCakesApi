namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class ProductType : BaseEntity<int>, ITranslatableEntity<ProductTypeTranslation>
    {
        //public string Name { get; set; } = default!;
        public ICollection<ProductTypeTranslation> Translations { get; set; } = [];
    }
}
