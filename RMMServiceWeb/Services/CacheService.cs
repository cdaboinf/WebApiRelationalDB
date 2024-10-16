using Microsoft.Extensions.Caching.Memory;
using RMMServiceWeb.Models.Cost;

namespace RMMServiceWeb.Services;

public class CacheService: ICacheService
{
    private readonly IMemoryCache _memoryCache;
    public CacheService(IMemoryCache memoryCache) =>
        _memoryCache = memoryCache;
    
    public void Cache<T>(string key, T value, TimeSpan expiration)
    {
        _memoryCache.Set(key, value, expiration);
    }

    public T? GetCache<T>(string key)
    {
        var value = _memoryCache.Get<T>(key);
        if (value == null)
        {
            return default;
        }
        return value;
    }
}