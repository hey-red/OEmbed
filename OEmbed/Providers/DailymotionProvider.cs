using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record DailymotionProvider : ProviderBase
{
    public DailymotionProvider()
    {
        AddAllowedHosts(new[] { "dailymotion.com", "www.dailymotion.com", "dai.ly" });

        AddScheme(
            new RegexMatcher(@"/(?:(?:embed/)?video/)?([\w]+)(?:[\w\?=]+)?"),
            "https://www.dailymotion.com/services/oembed",
            ResourceType.Video);
    }
}