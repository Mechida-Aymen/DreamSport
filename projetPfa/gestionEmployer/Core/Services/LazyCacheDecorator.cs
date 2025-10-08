using gestionEmployer.Core.Interfaces.CasheInterfaces;

namespace gestionEmployer.Core.Services
{
    public class LazyCacheDecorator : ICacheService
    {
        private readonly ICacheService _primaryCache;
        private readonly ICacheService _secondaryCache;

        public LazyCacheDecorator(ICacheService primaryCache, ICacheService secondaryCache)
        {
            _primaryCache = primaryCache;
            _secondaryCache = secondaryCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            // Try the primary cache first (L1 - In-memory)
            var result = await _primaryCache.GetAsync<T>(key);
            if (result != null)
                return result;

            result = await _secondaryCache.GetAsync<T>(key);
            if (result != null)
            {
                // Lazy populate L1 from L2
                await _primaryCache.SetAsync(key, result, TimeSpan.FromMinutes(5));
            }

            return result;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            // Set to both caches
            await _primaryCache.SetAsync(key, value, ttl);
            await _secondaryCache.SetAsync(key, value, ttl*12);
        }

        public async Task RemoveAsync(string key)
        {
            // Remove from both caches
            await _primaryCache.RemoveAsync(key);
            await _secondaryCache.RemoveAsync(key);
        }


        public T? Get<T>(string key)
        {
            var result = _primaryCache.Get<T>(key);
            if (result != null)
                return result;

            result = _secondaryCache.Get<T>(key);
            if (result != null)
            {
                _primaryCache.Set(key, result, TimeSpan.FromMinutes(5));
            }

            return result;
        }

        public void Set<T>(string key, T value, TimeSpan ttl)
        {
            _primaryCache.Set(key, value, ttl);
            _secondaryCache.Set(key, value, ttl * 12);
        }

        public void Remove(string key)
        {
            _primaryCache.Remove(key);
            _secondaryCache.Remove(key);
        }
    }
}
