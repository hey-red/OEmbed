using System;
using System.Threading.Tasks;
using System.Runtime.Caching;

using HeyRed.OEmbed.Abstractions;
using NeoSmart.Synchronization;
using HeyRed.OEmbed.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HeyRed.OEmbed.Defaults
{
    public class DefaultCache : ICache
    {
        private readonly ObjectCache _cache;

        private readonly ICacheKey _cacheKey;

        private readonly ILogger _logger;

        private readonly CacheOptions _options;

        public DefaultCache() : this(null, null, null)
        {
        }

        public DefaultCache(ICacheKey? cacheKey, ILoggerFactory? loggerFactory, CacheOptions? cacheOptions)
        {
            _cache = new MemoryCache("oembed");
            _cacheKey = cacheKey ?? new DefaultCacheKey();
            _logger = _logger =
                loggerFactory?.CreateLogger<OEmbedConsumer>() ??
                NullLoggerFactory.Instance.CreateLogger<OEmbedConsumer>();
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
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "An exception has occurred while processing request to url: {url}", url);

                        throw;
                    }
                    finally
                    {
                        if (item is T)
                        {
                            _cache.Set(key, item, _options.AbsoluteExpiration);
                        }
                        else
                        {
                            // Save empty object, because provider can return null/throw HttpRequestException
                            // This protects us against multiple request to invalid/not found urls
                            _cache.Set(key, _emptyValue, DateTimeOffset.UtcNow.AddMinutes(3));

                            _logger.LogDebug("Saved temporary placeholder object to avoid processing invalid/not found urls.");
                        }
                    }
                }
                else
                {
                    _logger.LogDebug(CACHED_LOG, url, key);
                }
            }
            else
            {
                _logger.LogDebug(CACHED_LOG, url, key);
            }

            return item is T obj ? obj : null;
        }

        private const string CACHED_LOG = "Return cached value. Url \"{url}\" with key \"{key}\".";

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