using MekkysCakes.Domain.Entities.WishlistModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class WishlistConfiguration : IEntityTypeConfiguration<Wishlist>
    {
        public void Configure(EntityTypeBuilder<Wishlist> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.UserEmail)
                .HasDatabaseName("IX_Wishlist_UserEmail");
        }
    }
}
