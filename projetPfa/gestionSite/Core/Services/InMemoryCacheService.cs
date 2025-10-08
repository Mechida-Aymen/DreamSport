using gestionSite.Core.Interfaces.CasheInterfaces;
using Microsoft.Extensions.Caching.Memory;

namespace gestionSite.Core.Services
{
    public class InMemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _cache;

        public InMemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T?> GetAsync<T>(string key)
        {
            if (_cache.TryGetValue(key, out T value))
            {
                return Task.FromResult(value);
            }
            return Task.FromResult<T?>(default);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            _cache.Set(key, value, ttl);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }

}
