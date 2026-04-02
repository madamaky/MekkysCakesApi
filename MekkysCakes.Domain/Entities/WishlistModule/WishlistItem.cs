using MekkysCakes.Domain.Entities.ProductModule;

namespace MekkysCakes.Domain.Entities.WishlistModule
{
    public class WishlistItem : BaseEntity<int>
    {
        public DateTime DateAdded { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public Product Product { get; set; } = default!;

        public Guid WishlistId { get; set; }
        public Wishlist Wishlist { get; set; } = default!;
    }
}
