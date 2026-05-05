using System.Text.Json;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities;
using MekkysCakes.Domain.Entities.ProductModule;
using MekkysCakes.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MekkysCakes.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InitializeAsync()
        {
            await SeedDataFromJsonAsync<ProductType, int>(_dbContext.ProductTypes, "productTypes.json");
            await SeedDataFromJsonAsync<ProductTheme, int>(_dbContext.ProductThemes, "productThemes.json");
            await SeedDataFromJsonAsync<Badge, int>(_dbContext.Badges, "badges.json");
            await _dbContext.SaveChangesAsync();

            await SeedDataFromJsonAsync<Product, int>(_dbContext.Products, "products.json");
            await _dbContext.SaveChangesAsync();
        }

        private async Task SeedDataFromJsonAsync<TEntity, Tkey>(DbSet<TEntity> dbSet, string fileName) where TEntity : BaseEntity<Tkey>
        {
            if (await dbSet.AnyAsync()) return;

            var filePath = @"..\MekkysCakes.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Couldn't find the file at the provided path");

            using var fileStream = File.OpenRead(filePath);
            var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(fileStream);
            if (data is not null)
                await dbSet.AddRangeAsync(data);
        }
    }
}
