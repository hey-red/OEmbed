using System;
using System.Threading.Tasks;
using System.Runtime.Caching;

using HeyRed.OEmbed.Abstractions;
using NeoSmart.Synchronization;

namespace HeyRed.OEmbed.Defaults
{
    public class DefaultCache : ICache
    {
        private readonly ObjectCache _cache;

        private readonly ICacheKey _cacheKey;

        private readonly CacheOptions _options;

        public DefaultCache() : this(null, null)
        {
        }

        public DefaultCache(ICacheKey? cacheKey, CacheOptions? cacheOptions)
        {
            _cache = new MemoryCache("oembed");
            _cacheKey = cacheKey ?? new DefaultCacheKey();
            _options = cacheOptions ?? new();
        }

        public async Task<T?> AddOrGetExistingAsync<T>(string url, Func<string, Task<T?>> task)
        {
            // Prevent to cache same urls
            if (url.EndsWith('/'))
            {
                url = url[0..^1];
            }

            var key = _cacheKey.CreateKey(url);

            var item = await GetAsync<T>(key);
            if (item == null)
            {
                using var mutex = await ScopedMutex.CreateAsync(key);
                // Double check
                item = await GetAsync<T>(key);
                if (item == null)
                {
                    try
                    {
                        item = await task(url);
                    }
                    catch
                    {
                        return default;
                    }

                    if (item is not null)
                    {
                        await SetAsync(key, item);
                    }
                }
            }

            return (T?)item;
        }

        public Task<T?> GetAsync<T>(string key) => Task.FromResult((T?)_cache.Get(key));

        public Task SetAsync<T>(string key, T item)
        {
            _cache.Set(key, item, _options.AbsoluteExpiration);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key) => Task.FromResult(_cache.Remove(key));
    }
}