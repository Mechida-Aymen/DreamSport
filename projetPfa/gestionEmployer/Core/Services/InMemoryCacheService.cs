using gestionEmployer.Core.Interfaces.CasheInterfaces;
using Microsoft.Extensions.Caching.Memory;

namespace gestionEmployer.Core.Services
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



        public T? Get<T>(string key)
        {
            _cache.TryGetValue(key, out T value);
            return value;
        }

        public void Set<T>(string key, T value, TimeSpan ttl)
        {
            _cache.Set(key, value, ttl);
        }

        public void Remove(string key)
        {
            _cache.Remove(key);
        }
    }

}
