using MekkysCakes.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class ProductBadgeConfiguration : IEntityTypeConfiguration<ProductBadge>
    {
        public void Configure(EntityTypeBuilder<ProductBadge> builder)
        {
            // Composite primary key
            builder.HasKey(pb => new { pb.ProductId, pb.BadgeId });

            builder.HasOne(pb => pb.Product)
                .WithMany(p => p.ProductBadges)
                .HasForeignKey(pb => pb.ProductId);
            
            builder.HasOne(pb => pb.Badge)
                .WithMany(b => b.ProductBadges)
                .HasForeignKey(pb => pb.BadgeId);
        }
    }
}
