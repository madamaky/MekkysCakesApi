using MekkysCakes.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class BadgeTranslationConfiguration : IEntityTypeConfiguration<BadgeTranslation>
    {
        public void Configure(EntityTypeBuilder<BadgeTranslation> builder)
        {
            builder.Property(t => t.Language).IsRequired().HasMaxLength(5);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            
            // One badge can have one translation per language
            builder.HasIndex(t => new { t.BadgeId, t.Language }).IsUnique();

            builder.HasOne(t => t.Badge)
                .WithMany(b => b.Translations)
                .HasForeignKey(t => t.BadgeId);
        }
    }
}
