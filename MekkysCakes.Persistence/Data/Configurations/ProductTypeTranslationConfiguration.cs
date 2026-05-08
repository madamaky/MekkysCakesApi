using MekkysCakes.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class ProductTypeTranslationConfiguration : IEntityTypeConfiguration<ProductTypeTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductTypeTranslation> builder)
        {
            builder.Property(t => t.Language).IsRequired().HasMaxLength(5);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

            // One product type can have one translation per language
            builder.HasIndex(t => new { t.ProductTypeId, t.Language }).IsUnique();
            
            builder.HasOne(t => t.ProductType)
                .WithMany(pt => pt.Translations)
                .HasForeignKey(t => t.ProductTypeId);
        }
    }
}
