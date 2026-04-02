namespace MekkysCakes.Services.Abstraction
{
    public interface ICacheService
    {
        Task<string?> GetAsync(string cacheKey);
        Task<bool> SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive);
    }
}
