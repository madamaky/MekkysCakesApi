namespace MekkysCakes.Domain.Contracts
{
    public interface ICacheRepository
    {
        Task<string?> GetAsync(string cacheKey);
        Task<bool> SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive);
    }
}
