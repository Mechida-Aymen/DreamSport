using gestionSite.Core.Interfaces.CasheInterfaces;
using Microsoft.EntityFrameworkCore.Storage;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text.Json;
using RedisDatabase = StackExchange.Redis.IDatabase;

namespace gestionSite.Core.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly RedisDatabase _db;
        private readonly JsonSerializerOptions _jsonOptions;

        public RedisCacheService(IConnectionMultiplexer connection)
        {
            _db = connection.GetDatabase();
            _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;

            return JsonSerializer.Deserialize<T>(value!, _jsonOptions);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan ttl)
        {
            var json = JsonSerializer.Serialize(value, _jsonOptions);
            await _db.StringSetAsync(key, json, ttl);
        }

        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }
    }
}
