using System;
using System.Threading.Tasks;

namespace HeyRed.OEmbed.Abstractions
{
    public interface ICache
    {
        Task<T?> GetAsync<T>(string key);

        Task SetAsync<T>(string key, T item);

        Task RemoveAsync(string key);

        Task<T?> AddOrGetExistingAsync<T>(string url, Func<string, Task<T?>> task);
    }
}