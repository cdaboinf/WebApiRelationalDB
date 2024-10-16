using RMMServiceWeb.Models.Cost;

namespace RMMServiceWeb.Services;

public interface ICacheService
{
    void Cache<T>(string key, T value, TimeSpan expiration);
    T? GetCache<T>(string key);
}