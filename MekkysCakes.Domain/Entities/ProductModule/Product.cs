namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public bool InStock { get; set; } = true;

        public decimal AverageRating { get; set; } = 0;
        public int TotalReviews { get; set; } = 0;

        #region Relationships

        public int ThemeId { get; set; }
        public ProductTheme ProductTheme { get; set; } = default!;

        public int TypeId { get; set; }
        public ProductType ProductType { get; set; } = default!;

        #endregion
    }
}
