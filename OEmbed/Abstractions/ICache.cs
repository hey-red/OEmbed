using System;
using System.Threading.Tasks;

using HeyRed.OEmbed.Models;

namespace HeyRed.OEmbed.Abstractions;

public interface ICache
{
    Task<T?> GetAsync<T>(string key) where T : Base;

    Task SetAsync<T>(string key, T item) where T : Base;

    Task RemoveAsync(string key);

    Task<T?> AddOrGetExistingAsync<T>(string url, Func<string, Task<T?>> task) where T : Base;
}