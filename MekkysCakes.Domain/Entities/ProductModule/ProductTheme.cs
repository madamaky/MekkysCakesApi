namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class ProductTheme : BaseEntity<int>, ITranslatableEntity<ProductThemeTranslation>
    {
        //public string Name { get; set; } = default!;
        public ICollection<ProductThemeTranslation> Translations { get; set; } = [];
    }
}
