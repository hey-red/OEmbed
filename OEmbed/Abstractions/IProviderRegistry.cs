using System;

namespace HeyRed.OEmbed.Abstractions;

public interface IProviderRegistry
{
    OEmbedProviderInfo? GetProvider(Uri uri);

    OEmbedProviderInfo? GetProvider(string url);
}