using System;
using System.Collections.Generic;
using System.Linq;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed;

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
            ProviderScheme? scheme = provider.MatchScheme(uri);
            if (scheme is not null)
            {
                return new OEmbedProviderInfo(
                    scheme,
                    provider.Format,
                    provider.Parameters,
                    provider.PreProcessUrl);
            }
        }

        return null;
    }

    public OEmbedProviderInfo? GetProvider(string url)
    {
        return GetProvider(new Uri(url));
    }
}