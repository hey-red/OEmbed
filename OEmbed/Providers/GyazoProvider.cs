using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

/// <summary>
///     https://gyazo.com/api/docs/image#oembed
/// </summary>
public record GyazoProvider : ProviderBase
{
    public GyazoProvider()
    {
        AddAllowedHosts(new[] { "gyazo.com", "www.gyazo.com" });

        AddScheme(
            new RegexMatcher(@"/(\S{32,40})"),
            "https://api.gyazo.com/api/oembed",
            ResourceType.Photo);
    }
}