using MekkysCakes.Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class BadgeConfiguration : IEntityTypeConfiguration<Badge>
    {
        public void Configure(EntityTypeBuilder<Badge> builder)
        {
            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(b => b.Name)
                .IsUnique();
        }
    }
}
