using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record AnnieMusicProvider : ProviderBase
{
    public AnnieMusicProvider()
    {
        AddAllowedHosts(new[] { "anniemusic.app" });

        AddScheme(
            new RegexMatcher(@"/(?:t|p)/([\w]+)([^/^\s]+)?"),
            "https://api.anniemusic.app/api/v1/oembed",
            ResourceType.Rich);
    }
}