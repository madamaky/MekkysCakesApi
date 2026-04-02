using MekkysCakes.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(100);

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.PictureUrl)
                .HasMaxLength(200);

            builder.Property(p => p.Price)
                .HasPrecision(18, 2);

            builder.HasOne(p => p.ProductTheme)
                .WithMany()
                .HasForeignKey(p => p.ThemeId);

            builder.HasOne(p => p.ProductType)
                .WithMany()
                .HasForeignKey(p => p.TypeId);
        }
    }
}
