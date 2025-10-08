namespace gestionEmployer.Core.Interfaces.CasheInterfaces
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan ttl);
        Task RemoveAsync(string key);

        T? Get<T>(string key);
        void Set<T>(string key, T value, TimeSpan ttl);
        void Remove(string key);
    }
}
