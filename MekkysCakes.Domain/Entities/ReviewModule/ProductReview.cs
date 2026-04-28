using MekkysCakes.Domain.Entities.IdentityModule;
using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Domain.Entities.ReviewModule
{
    public class ProductReview : BaseEntity<int>
    {
        public int Rating { get; set; }
        public string? Title { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsApproved { get; set; }

        #region Relationships

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;

        #endregion
    }
}
