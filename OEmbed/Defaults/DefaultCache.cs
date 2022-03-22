using System;
using System.Threading.Tasks;
using System.Runtime.Caching;

using HeyRed.OEmbed.Abstractions;
using NeoSmart.Synchronization;
using HeyRed.OEmbed.Models;

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
            where T : Base
        {
            // Prevent to cache same urls
            if (url.EndsWith('/'))
            {
                url = url[0..^1];
            }

            var key = _cacheKey.CreateKey(url);

            object? item = _cache.Get(key);
            if (item is null)
            {
                using var mutex = await ScopedMutex.CreateAsync(key);
                // Double check
                item = _cache.Get(key);
                if (item is null)
                {
                    try
                    {
                        item = await task(url);
                    }
                    catch
                    {
                        // TODO: logger
                    }

                    if (item is T)
                    {
                        _cache.Set(key, item, _options.AbsoluteExpiration);
                    }
                    else
                    {
                        // Save empty object, because provider can return null/throw HttpRequestException
                        // This protects us against multiple request to invalid/not found urls
                        _cache.Set(key, _emptyValue, DateTimeOffset.UtcNow.AddMinutes(3));
                    }
                }
            }

            return item is T obj ? obj : null;
        }

        private static readonly object _emptyValue = new();

        public Task<T?> GetAsync<T>(string key) where T : Base
        {
            var value = _cache.Get(key);

            return value is T obj ?
                Task.FromResult<T?>(obj) :
                Task.FromResult<T?>(null);
        }

        public Task SetAsync<T>(string key, T item) where T : Base
        {
            _cache.Set(key, item, _options.AbsoluteExpiration);

            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key) => Task.FromResult(_cache.Remove(key));
    }
}