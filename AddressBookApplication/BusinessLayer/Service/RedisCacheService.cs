using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BusinessLayer.Service
{
    public class RedisCacheService
    {
        private readonly IDistributedCache _cache;

        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public async Task SetCacheAsync<T>(string key, T value, int expirationMinutes = 10)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationMinutes)
            };

            var serializedData = JsonSerializer.Serialize(value);
            Console.WriteLine($"Caching data: {serializedData}");
            await _cache.SetStringAsync(key, serializedData, options);
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            var jsonData = await _cache.GetStringAsync(key);
            return jsonData != null ? JsonSerializer.Deserialize<T>(jsonData) : default;
        }

        public async Task RemoveCacheAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
