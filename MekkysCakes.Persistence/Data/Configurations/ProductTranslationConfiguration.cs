using MekkysCakes.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class ProductTranslationConfiguration : IEntityTypeConfiguration<ProductTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTranslation> builder)
        {
            builder.Property(t => t.Language).IsRequired().HasMaxLength(5);
             builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
            builder.Property(t => t.Description).IsRequired().HasMaxLength(500);

            // One product can have one translation per language
            builder.HasIndex(t => new { t.ProductId, t.Language }).IsUnique();

            builder.HasOne(t => t.Product)
                .WithMany(p => p.Translations)
                .HasForeignKey(t => t.ProductId);
        }
    }
}