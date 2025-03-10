﻿using Infrastructure.Utilities.Caching.Abstractions;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Utilities.Caching.Implementations
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache? _cache;

        public RedisCacheService(IDistributedCache? cache)
        {
            _cache = cache;
        }

        public T? GetData<T>(string key)
        {
            var data = _cache?.GetString(key);
            if (data == null)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(data)!;
        }

        public void SetData<T>(string key, T data)
        {
            var option = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            };
            _cache?.SetString(key, JsonSerializer.Serialize(data), option);
        }

        public void RemoveData(string key)
        {
            _cache?.Remove(key);
        }
    }
}
