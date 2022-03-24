using System;
using System.Collections.Generic;
using System.Linq;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed
{
    public class ProviderRegistry : IProviderRegistry
    {
        private readonly IEnumerable<IOEmbedProvider> _oEmbedProviders;

        public ProviderRegistry(IEnumerable<IOEmbedProvider> oEmbedProviders)
        {
            _oEmbedProviders = oEmbedProviders.EnsureNotNull();
        }

        public OEmbedProviderInfo? GetProvider(Uri uri)
        {
            uri.EnsureNotNull();

            IOEmbedProvider? provider = _oEmbedProviders.FirstOrDefault(pr => pr.CanProcess(uri));
            if (provider is not null)
            {
                var scheme = provider.MatchScheme(uri);
                if (scheme is not null)
                {
                    return new OEmbedProviderInfo(
                        Scheme: scheme,
                        ResponseFormat: provider.Format,
                        Parameters: provider.Parameters);
                }
            }
            return null;
        }

        public OEmbedProviderInfo? GetProvider(string url) => GetProvider(new Uri(url));
    }
}