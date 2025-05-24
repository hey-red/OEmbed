using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record AudiomackProvider : ProviderBase
{
    public AudiomackProvider()
    {
        AddAllowedHosts(new[] { "audiomack.com" });

        AddScheme(
            new RegexMatcher(
                @"/(?:[\w\-]+)/(?:song|album|playlist)/([\w\-]+)/?",
                @"/(?:song|album|playlist)/(?:[\w\-]+)/([\w\-]+)/?"),
            "https://www.audiomack.com/oembed",
            ResourceType.Rich);
    }
}