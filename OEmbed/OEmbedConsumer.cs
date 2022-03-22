﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Defaults;
using HeyRed.OEmbed.Models;
using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed
{
    public class OEmbedConsumer : IOEmbedConsumer
    {
        private readonly HttpClient _httpClient;

        private readonly IProviderRegistry _providerRegistry;

        private readonly IJsonSerializer _jsonSerializer;

        private readonly IXmlSerializer _xmlSerializer;

        private readonly ICache _cache;

        private readonly OEmbedOptions _options;

        private static readonly object _initLock = new();

        public OEmbedConsumer(
            HttpClient httpClient,
            IProviderRegistry providerRegistry,
            IJsonSerializer? jsonSerializer = null,
            IXmlSerializer? xmlSerializer = null,
            ICache? cache = null,
            OEmbedOptions? options = null)
        {
            _httpClient = httpClient.EnsureNotNull();
            _providerRegistry = providerRegistry.EnsureNotNull();
            _jsonSerializer = jsonSerializer ?? new DefaultJsonSerializer();
            _xmlSerializer = xmlSerializer ?? new DefaultXmlSerializer();
            _cache = cache ?? new DefaultCache();
            _options = options ?? new();

            if (_httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
            {
                lock (_initLock)
                {
                    if (_httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
                    {
                        var type = typeof(OEmbedConsumer);
                        var assemblyName = type.Assembly.GetName();
                        var libName = type.Namespace + "/" + assemblyName!.Version!.Major + "." + assemblyName!.Version!.Minor;
                        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(libName);
                    }
                }
            }
        }

        /// <summary>
        /// Makes request to specific url and deserialize response.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private async Task<T?> DoRequestAsync<T>(string requestUrl, CancellationToken cancellationToken) where T : Base
        {
            using var response = await _httpClient.GetAsync(requestUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            return
                response.Content.Headers.ContentType?.MediaType == "text/xml" ?
                    _xmlSerializer.Deserialize<T>(responseStream) :
                    _jsonSerializer.Deserialize<T>(responseStream);
        }

        /// <summary>
        /// Makes request to specific api endpoint with consumer request.
        /// </summary>
        /// <param name="apiEndpoint">The URL of the service.</param>
        /// <param name="request">The Request sent to the API endpoint.</param>
        /// <returns>returns <see cref="Base"/></returns>
        /// <exception cref="HttpRequestException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<T?> RequestAsync<T>(
            Uri apiEndpoint,
            IOEmbedConsumerRequest request,
            IEnumerable<KeyValuePair<string, string?>>? parameters = default,
            CancellationToken cancellationToken = default)
            where T : Base
        {
            apiEndpoint.EnsureNotNull();
            request.EnsureNotNull();

            string requestUrl = apiEndpoint + request.ToString();
            // Append query parameters
            if (parameters != null)
            {
                requestUrl = UrlHelpers.AddQueryString(requestUrl, parameters);
            }

            return _options.EnableCache ?
                await _cache.AddOrGetExistingAsync(requestUrl, async (requestUrl) => await DoRequestAsync<T>(requestUrl, cancellationToken)) :
                await DoRequestAsync<T>(requestUrl, cancellationToken);
        }

        /// <summary>
        /// Finds provider for given uri and makes request to api endpoint with consumer request.
        /// </summary>
        /// <param name="uri"></param>
        /// <returns>returns <see cref="Base"/> or null if provider not found for given uri.</returns>
        public async Task<T?> RequestAsync<T>(
            Uri uri,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default)
            where T : Base
        {
            if (uri.IsFile) return null;

            OEmbedProviderInfo? providerInfo = _providerRegistry.GetProvider(uri);
            if (providerInfo is not null)
            {
                var consumerRequest = new OEmbedConsumerRequest(uri, providerInfo.ResponseFormat, maxWidth, maxHeight);

                return await RequestAsync<T>(
                    providerInfo.Scheme.Endpoint,
                    consumerRequest,
                    parameters: providerInfo.Parameters,
                    cancellationToken: cancellationToken);
            }

            return null;
        }

        /// <summary>
        /// Finds provider for given url and makes request to api endpoint with consumer request.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>returns <see cref="Base"/> or null if provider not found for given url.</returns>
        public async Task<T?> RequestAsync<T>(
            string url,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default)
            where T : Base
        {
            if (Uri.TryCreate(url, uriKind: UriKind.Absolute, out var uri) && uri.IsWellFormedOriginalString())
            {
                return await RequestAsync<T>(uri, maxWidth, maxHeight, cancellationToken);
            }

            return null;
        }

        /// <summary>
        /// Finds provider for given uri and makes request to api endpoint with consumer request.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns>returns dynamic or null if provider not found for given url.</returns>
        /// <remarks>Use this method if you don't know type of resource response.</remarks>
        public async Task<dynamic?> RequestAsync(
            Uri uri,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default)
        {
            if (uri.IsFile) return null;

            OEmbedProviderInfo? providerInfo = _providerRegistry.GetProvider(uri);
            if (providerInfo is not null)
            {
                var consumerRequest = new OEmbedConsumerRequest(uri, providerInfo.ResponseFormat, maxWidth, maxHeight);

                ProviderScheme scheme = providerInfo.Scheme;

                return scheme.ResourceType switch
                {
                    ResourceType.Video => await RequestAsync<Video>(scheme.Endpoint, consumerRequest, providerInfo.Parameters, cancellationToken),
                    ResourceType.Photo => await RequestAsync<Photo>(scheme.Endpoint, consumerRequest, providerInfo.Parameters, cancellationToken),
                    ResourceType.Rich => await RequestAsync<Rich>(scheme.Endpoint, consumerRequest, providerInfo.Parameters, cancellationToken),
                    ResourceType.Link => await RequestAsync<Link>(scheme.Endpoint, consumerRequest, providerInfo.Parameters, cancellationToken),
                    _ => await RequestAsync<Base>(scheme.Endpoint, consumerRequest, providerInfo.Parameters, cancellationToken),
                };
            }

            return null;
        }

        /// <summary>
        /// Finds provider for given url and makes request to api endpoint with consumer request.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns>returns <see cref="Base"/> or null if provider not found for given url.</returns>
        /// <remarks>Use this method if you don't know type of resource response.</remarks>
        public async Task<dynamic?> RequestAsync(
            string url,
            int? maxWidth = null,
            int? maxHeight = null,
            CancellationToken cancellationToken = default)
        {
            if (Uri.TryCreate(url, uriKind: UriKind.Absolute, out var uri) && uri.IsWellFormedOriginalString())
            {
                return await RequestAsync(uri, maxWidth, maxHeight, cancellationToken);
            }

            return null;
        }
    }
}