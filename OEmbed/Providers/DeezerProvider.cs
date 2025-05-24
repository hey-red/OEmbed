using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record DeezerProvider : ProviderBase
{
    public DeezerProvider()
    {
        AddAllowedHosts(new[] { "www.deezer.com", "deezer.com", "deezer.page.link" });

        AddScheme(
            new RegexMatcher(
                @"/(?:\w+/)?(track|playlist|album)/(\d+)/?",
                @"/(\w+)/?"),
            "http://api.deezer.com/oembed",
            ResourceType.Rich);
    }
}