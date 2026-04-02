using System.Text.Json;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Domain.Entities.BasketModule;
using StackExchange.Redis;

namespace MekkysCakes.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket?> GetBasketAsync(string basketId)
        {
            var basket = await _database.StringGetAsync(basketId);
            if (basket.IsNullOrEmpty)
                return null;

            return JsonSerializer.Deserialize<CustomerBasket>(basket!);
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan timeToLive = default)
        {
            var jsonBasket = JsonSerializer.Serialize(basket);

            var isCreatedOrUpdated = await _database.StringSetAsync(basket.Id, jsonBasket, (timeToLive == default) ? TimeSpan.FromDays(1) : timeToLive);
            if (isCreatedOrUpdated)
                return await GetBasketAsync(basket.Id);

            return null;
        }

        public async Task<bool> DeleteBasketAsync(string basketId) => await _database.KeyDeleteAsync(basketId);
    }
}
