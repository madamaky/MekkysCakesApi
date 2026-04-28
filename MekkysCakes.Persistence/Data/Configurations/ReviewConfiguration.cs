using MekkysCakes.Domain.Entities.ReviewModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MekkysCakes.Persistence.Data.Configurations
{
    public class ReviewConfiguration : IEntityTypeConfiguration<ProductReview>
    {
        public void Configure(EntityTypeBuilder<ProductReview> builder)
        {
            builder.Property(r => r.Rating).IsRequired();

            builder.Property(r => r.Title).HasMaxLength(150);
            builder.Property(r => r.Comment).HasMaxLength(2000);
            builder.Property(r => r.UserId).HasMaxLength(450).IsRequired();

            // Relationships
            builder.HasOne(r => r.Product)
                .WithMany()
                .HasForeignKey(r => r.ProductId)
                .OnDelete(DeleteBehavior.Cascade); // Delete product => delete its reviews

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Delete user → restrict deletion of their reviews

            // Unique constraint: one review per user per product
            builder.HasIndex(r => new { r.ProductId, r.UserId })
                .IsUnique()
                .HasDatabaseName("IX_ProductReview_UserProduct");

            // Index for "get all reviews for product X, newest first"
            builder.HasIndex(r => new { r.ProductId, r.CreatedAt })
                .HasDatabaseName("IX_ProductReview_ProductDate");
        }
    }
}
