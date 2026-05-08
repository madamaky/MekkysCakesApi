using MekkysCakes.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class ProductThemeTranslationConfiguration : IEntityTypeConfiguration<ProductThemeTranslation>
    {
        public void Configure(EntityTypeBuilder<ProductThemeTranslation> builder)
        {
            builder.Property(t => t.Language).IsRequired().HasMaxLength(5);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(100);

            // One product theme can have one translation per language
            builder.HasIndex(t => new { t.ProductThemeId, t.Language }).IsUnique();
            
            builder.HasOne(t => t.ProductTheme)
                .WithMany(pt => pt.Translations)
                .HasForeignKey(t => t.ProductThemeId);
        }
    }
}
