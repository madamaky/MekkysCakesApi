using MekkysCakes.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class DeliveryMethodTranslationConfiguration : IEntityTypeConfiguration<DeliveryMethodTranslation>
    {
        public void Configure(EntityTypeBuilder<DeliveryMethodTranslation> builder)
        {
            builder.Property(t => t.Language).IsRequired().HasMaxLength(5);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            builder.Property(t => t.Description).HasMaxLength(100);
            builder.Property(t => t.DeliveryTime).HasMaxLength(50);

            // One delivery method can have one translation per language
            builder.HasIndex(t => new { t.DeliveryMethodId, t.Language }).IsUnique();

            builder.HasOne(t => t.DeliveryMethod)
                .WithMany(dm => dm.Translations)
                .HasForeignKey(t => t.DeliveryMethodId);
        }
    }
}
