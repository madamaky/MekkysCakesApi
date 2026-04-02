namespace MekkysCakes.Domain.Entities.WishlistModule
{
    public class Wishlist : BaseEntity<Guid>
    {
        public string UserEmail { get; set; } = default!;
        public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
    }
}
