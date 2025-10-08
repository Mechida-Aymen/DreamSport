using gestionSite.Core.Interfaces.CasheInterfaces;

namespace gestionSite.Core.Services
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

            // If not found in primary, check the secondary cache (L2 - Redis)
            return await _secondaryCache.GetAsync<T>(key);
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
    }
}
