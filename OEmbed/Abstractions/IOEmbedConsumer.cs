using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using HeyRed.OEmbed.Models;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IOEmbedConsumer
    {
        Task<T?> RequestAsync<T>(
            Uri apiEndpoint,
            IOEmbedConsumerRequest request,
            IEnumerable<KeyValuePair<string, string?>>? parameters = default,
            CancellationToken cancellationToken = default)
            where T : Base;

        Task<T?> RequestAsync<T>(
            Uri uri,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default)
            where T : Base;

        Task<T?> RequestAsync<T>(
            string url,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default)
            where T : Base;

        Task<dynamic?> RequestAsync(
            Uri uri,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default);

        Task<dynamic?> RequestAsync(
            string url,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default);
    }
}