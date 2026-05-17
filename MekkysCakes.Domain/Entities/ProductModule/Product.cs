namespace MekkysCakes.Domain.Entities.ProductModule
{
    public class Product : BaseEntity<int>, ITranslatableEntity<ProductTranslation>
    {
        //public string Name { get; set; } = default!;
        //public string Description { get; set; } = default!;
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

        public ICollection<ProductBadge> ProductBadges { get; set; } = [];

        public ICollection<ProductTranslation> Translations { get; set; } = [];

        #endregion


        #region Helper Methods

        public void IncludeReviewInRatings(int rating)
        {
            TotalReviews += 1;
            AverageRating = ((AverageRating * (TotalReviews - 1)) + rating) / TotalReviews;
        }

        public void ExcludeReviewFromRatings(int rating)
        {
            if (TotalReviews <= 0) return;

            if (TotalReviews == 1)
            {
                AverageRating = 0;
                TotalReviews = 0;
            }
            else
            {
                AverageRating = (AverageRating * TotalReviews - rating) / (TotalReviews - 1);
                TotalReviews -= 1;
            }
        }

        public void UpdateReviewRating(int oldRating, int newRating)
        {
            if (TotalReviews <= 0) return;
            AverageRating = (AverageRating * TotalReviews - oldRating + newRating) / TotalReviews;
        }

        #endregion
    }
}
