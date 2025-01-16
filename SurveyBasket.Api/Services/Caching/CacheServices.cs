using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurveyBasket.Api.Services.Caching;

public class CacheServices(IDistributedCache distributedCache) : ICacheServices
{
    private readonly IDistributedCache _distributedCache = distributedCache;

    public async Task<T?> GetAsync<T>(string Key, CancellationToken cancellationToken = default) where T : class
    {
        var cachedValue = await _distributedCache.GetStringAsync(Key, cancellationToken);

        return string.IsNullOrEmpty(cachedValue)
            ? null
            : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task SetAsync<T>(string Key, T value, CancellationToken cancellationToken = default) where T : class
    {
        await _distributedCache.SetStringAsync(Key, JsonSerializer.Serialize(value), cancellationToken);
    }

    public async Task RemoveAsync(string Key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(Key, cancellationToken);
    }
}
