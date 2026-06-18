using MekkysCakes.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.ContactEmail)
            .IsRequired()
            .HasMaxLength(256);

            builder.Property(o => o.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(o => o.Address)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(o => o.SubTotal)
                .HasPrecision(18, 2);

            builder.Property(o => o.OrderStatus)
                .HasConversion<string>(); // store as "Pending" not 0

            // User FK — no nav prop on ApplicationUser side
            builder.HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict); // don't cascade-delete orders on user deletion

            builder.HasOne(o => o.DeliveryMethod)
                .WithMany()
                .HasForeignKey(o => o.DeliveryMethodId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(o => o.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade); // items are owned by the order
        }
    }
}
