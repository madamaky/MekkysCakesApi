using System.Reflection;
using MekkysCakes.Domain.Entities.OrderModule;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Domain.Entities.WishlistModule;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Persistence.Data.DbContexts
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProductConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTheme> ProductThemes { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
