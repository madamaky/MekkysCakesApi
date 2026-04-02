using MekkysCakes.Domain.Entities.WishlistModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    internal class WishlistItemConfiguration : IEntityTypeConfiguration<WishlistItem>
    {
        public void Configure(EntityTypeBuilder<WishlistItem> builder)
        {
            builder.HasKey(wi => wi.Id);

            // Explicitly link the Navigation Property to the Foreign Key property
            builder.HasOne(wi => wi.Wishlist)      // The Class property
                  .WithMany(w => w.Items)         // The Collection on the other side
                  .HasForeignKey(wi => wi.WishlistId) // The ID property in WishlistItem
                  .OnDelete(DeleteBehavior.Cascade);

            // Foreign Key to Product
            builder.HasOne(wi => wi.Product)
                  .WithMany()
                  .HasForeignKey(wi => wi.ProductId);

            // Performance: Index the WishlistId since it repeats
            // This makes joining the two tables much faster
            builder.HasIndex(wi => wi.WishlistId)
                  .HasDatabaseName("IX_WishlistItem_WishlistId");

            // Business Rule: Prevent duplicate products in the same wishlist
            builder.HasIndex(wi => new { wi.WishlistId, wi.ProductId })
                  .IsUnique();
        }
    }
}
