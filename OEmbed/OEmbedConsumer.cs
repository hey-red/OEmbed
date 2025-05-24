using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Defaults;
using HeyRed.OEmbed.Models;
using HeyRed.OEmbed.Providers.Common;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace HeyRed.OEmbed;

public class OEmbedConsumer : IOEmbedConsumer
{
    private static readonly object _initLock = new();

    private readonly ICache? _cache;
    private readonly HttpClient _httpClient;

    private readonly IJsonSerializer _jsonSerializer;

    private readonly ILogger _logger;

    private readonly OEmbedOptions _options;

    private readonly IProviderRegistry _providerRegistry;

    private readonly IXmlSerializer _xmlSerializer;

    public OEmbedConsumer(
        HttpClient httpClient,
        IProviderRegistry providerRegistry,
        IJsonSerializer? jsonSerializer = null,
        IXmlSerializer? xmlSerializer = null,
        ICache? cache = null,
        ILoggerFactory? loggerFactory = null,
        OEmbedOptions? options = null)
    {
        _httpClient = httpClient.EnsureNotNull();
        _providerRegistry = providerRegistry.EnsureNotNull();
        _jsonSerializer = jsonSerializer ?? new DefaultJsonSerializer();
        _xmlSerializer = xmlSerializer ?? new DefaultXmlSerializer();
        _options = options ?? new OEmbedOptions();

        _logger =
            loggerFactory?.CreateLogger<OEmbedConsumer>() ??
            NullLoggerFactory.Instance.CreateLogger<OEmbedConsumer>();

        if (_options.EnableCache)
        {
            _cache = cache ?? new DefaultCache();
        }

        if (_httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
        {
            lock (_initLock)
            {
                if (_httpClient.DefaultRequestHeaders.UserAgent.Count == 0)
                {
                    Type type = typeof(OEmbedConsumer);
                    AssemblyName assemblyName = type.Assembly.GetName();
                    string libName = type.Namespace +
                                     "/" +
                                     assemblyName!.Version!.Major +
                                     "." +
                                     assemblyName!.Version!.Minor;
                    _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(libName);
                }
            }
        }
    }

    /// <summary>
    ///     Makes request to specific api endpoint with consumer request.
    /// </summary>
    /// <param name="apiEndpoint">The URL of the service.</param>
    /// <param name="request">The Request sent to the API endpoint.</param>
    /// <param name="parameters">Additional query parameters.</param>
    /// <returns>returns <see cref="Base" /> or null</returns>
    public async Task<T?> RequestAsync<T>(
        Uri apiEndpoint,
        IOEmbedConsumerRequest request,
        IEnumerable<KeyValuePair<string, string?>>? parameters = default,
        CancellationToken cancellationToken = default)
        where T : Base
    {
        apiEndpoint.EnsureNotNull();
        request.EnsureNotNull();

        string requestUrl = UrlHelpers.AddQueryString(apiEndpoint.OriginalString, request.BuildQueryParams());

        // Append query parameters
        if (parameters != null)
        {
            requestUrl = UrlHelpers.AddQueryString(requestUrl, parameters);
        }

        _logger.LogDebug("Request url: {requestUrl}", requestUrl);

        return _options.EnableCache
            ? await _cache!.AddOrGetExistingAsync(requestUrl,
                async requestUrl => await DoRequestAsync<T>(requestUrl, cancellationToken))
            : await DoRequestAsync<T>(requestUrl, cancellationToken);
    }

    /// <summary>
    ///     Finds provider for given uri and makes request to api endpoint with consumer request.
    /// </summary>
    /// <param name="uri"></param>
    /// <returns>returns <see cref="Base" /> or null if provider not found for given uri.</returns>
    public async Task<T?> RequestAsync<T>(
        Uri uri,
        int? maxWidth = null,
        int? maxHeight = null,
        CancellationToken cancellationToken = default)
        where T : Base
    {
        GuardAgainstInvalidUri(uri);

        OEmbedProviderInfo? providerInfo = _providerRegistry.GetProvider(uri);
        if (providerInfo is not null)
        {
            var consumerRequest = new OEmbedConsumerRequest(providerInfo.UrlPreProcessor?.Invoke(uri) ?? uri,
                providerInfo.ResponseFormat, maxWidth, maxHeight);

            return await RequestAsync<T>(
                providerInfo.Scheme.Endpoint,
                consumerRequest,
                providerInfo.Parameters,
                cancellationToken);
        }

        return null;
    }

    /// <summary>
    ///     Finds provider for given url and makes request to api endpoint with consumer request.
    /// </summary>
    /// <param name="url"></param>
    /// <returns>returns <see cref="Base" /> or null if provider not found for given url.</returns>
    public async Task<T?> RequestAsync<T>(
        string url,
        int? maxWidth = null,
        int? maxHeight = null,
        CancellationToken cancellationToken = default)
        where T : Base
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return await RequestAsync<T>(uri, maxWidth, maxHeight, cancellationToken);
        }

        _logger.LogDebug("Invalid url \"{url}\". Skip processing.", url);

        return null;
    }

    /// <summary>
    ///     Finds provider for given uri and makes request to api endpoint with consumer request.
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="maxWidth"></param>
    /// <param name="maxHeight"></param>
    /// <returns>returns dynamic or null if provider not found for given url.</returns>
    /// <remarks>Use this method if you don't know type of resource response.</remarks>
    public async Task<Base?> RequestAsync(
        Uri uri,
        int? maxWidth = null,
        int? maxHeight = null,
        CancellationToken cancellationToken = default)
    {
        GuardAgainstInvalidUri(uri);

        OEmbedProviderInfo? providerInfo = _providerRegistry.GetProvider(uri);
        if (providerInfo is not null)
        {
            var consumerRequest = new OEmbedConsumerRequest(
                providerInfo.UrlPreProcessor?.Invoke(uri) ?? uri, providerInfo.ResponseFormat,
                maxWidth,
                maxHeight);

            ProviderScheme scheme = providerInfo.Scheme;

            return scheme.ResourceType switch
            {
                ResourceType.Video => await RequestAsync<Video>(scheme.Endpoint, consumerRequest,
                    providerInfo.Parameters, cancellationToken),
                ResourceType.Photo => await RequestAsync<Photo>(scheme.Endpoint, consumerRequest,
                    providerInfo.Parameters, cancellationToken),
                ResourceType.Rich => await RequestAsync<Rich>(scheme.Endpoint, consumerRequest, providerInfo.Parameters,
                    cancellationToken),
                ResourceType.Link => await RequestAsync<Link>(scheme.Endpoint, consumerRequest, providerInfo.Parameters,
                    cancellationToken),
                _ => await RequestAsync<Base>(scheme.Endpoint, consumerRequest, providerInfo.Parameters,
                    cancellationToken)
            };
        }

        return null;
    }

    /// <summary>
    ///     Finds provider for given url and makes request to api endpoint with consumer request.
    /// </summary>
    /// <param name="url"></param>
    /// <param name="maxWidth"></param>
    /// <param name="maxHeight"></param>
    /// <returns>returns <see cref="Base" /> or null if provider not found for given url.</returns>
    /// <remarks>Use this method if you don't know type of resource response.</remarks>
    public async Task<Base?> RequestAsync(
        string url,
        int? maxWidth = null,
        int? maxHeight = null,
        CancellationToken cancellationToken = default)
    {
        if (Uri.TryCreate(url, UriKind.Absolute, out Uri? uri))
        {
            return await RequestAsync(uri, maxWidth, maxHeight, cancellationToken);
        }

        _logger.LogDebug("Invalid url \"{url}\". Skip processing.", url);

        return null;
    }

    private static void GuardAgainstInvalidUri(Uri uri)
    {
        if (!UrlHelpers.IsValidUri(uri))
        {
            throw new ArgumentException("The request uri should be well-formed and starts with http:// or https://");
        }
    }

    /// <summary>
    ///     Makes request to specific url and deserialize response.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="requestUrl"></param>
    private async Task<T?> DoRequestAsync<T>(string requestUrl, CancellationToken cancellationToken) where T : Base
    {
        using HttpResponseMessage response = await _httpClient.GetAsync(requestUrl, cancellationToken);
        response.EnsureSuccessStatusCode();

        using Stream responseStream = await response.Content.ReadAsStreamAsync(cancellationToken);

        // Select serializer based on resource response content type.
        string? mediaType = response.Content.Headers.ContentType?.MediaType;
        return
            mediaType switch
            {
                "text/xml" => _xmlSerializer.Deserialize<T>(responseStream),
                "application/json" => _jsonSerializer.Deserialize<T>(responseStream),
                _ => throw new InvalidOperationException($"Unsupported response content type: {mediaType}")
            };
    }
}