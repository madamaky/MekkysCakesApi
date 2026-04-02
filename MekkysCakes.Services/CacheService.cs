using System.Text.Json;
using MekkysCakes.Domain.Contracts;
using MekkysCakes.Services.Abstraction;

namespace MekkysCakes.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }

        public async Task<string?> GetAsync(string cacheKey)
            => await _cacheRepository.GetAsync(cacheKey);

        public async Task<bool> SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive)
        {
            var stringValue = JsonSerializer.Serialize(cacheValue, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            return await _cacheRepository.SetAsync(cacheKey, stringValue, timeToLive);
        }
    }
}
