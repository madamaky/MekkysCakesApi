namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class ProductThemeTranslation : BaseEntity<int>, ITranslation
    {
        public string Language { get; set; } = default!;
        public string Name { get; set; } = default!;

        public int ProductThemeId { get; set; }
        public ProductTheme ProductTheme { get; set; } = default!;
    }
}