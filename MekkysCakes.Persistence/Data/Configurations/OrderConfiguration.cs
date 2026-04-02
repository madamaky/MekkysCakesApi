using MekkysCakes.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(x => x.SubTotal)
                .HasPrecision(8, 2);

            builder.OwnsOne(x => x.Address, oEntity =>
            {
                oEntity.Property(x => x.FirstName).HasMaxLength(50);
                oEntity.Property(x => x.LastName).HasMaxLength(50);
                oEntity.Property(x => x.Street).HasMaxLength(50);
                oEntity.Property(x => x.City).HasMaxLength(50);
            });
        }
    }
}
