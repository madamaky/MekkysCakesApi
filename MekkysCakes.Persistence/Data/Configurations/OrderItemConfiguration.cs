using MekkysCakes.Domain.Entities.OrderModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price).HasPrecision(8, 2);

            builder.OwnsOne(x => x.Product, oEntity =>
            {
                oEntity.Property(x => x.ProductName).HasMaxLength(100);
                oEntity.Property(x => x.PictureUrl).HasMaxLength(200);
            });

            builder.HasOne(orderItem => orderItem.Order)
                .WithMany(order => order.Items)
                .HasForeignKey(orderItem => orderItem.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
