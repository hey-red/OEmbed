using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record TiktokProvider : ProviderBase
{
    public TiktokProvider()
    {
        AddAllowedHosts(new[]
        {
            "tiktok.com",
            "www.tiktok.com",
            "m.tiktok.com"
        });

        // https://developers.tiktok.com/doc/embed-creator-profiles/
        AddScheme(
            new RegexMatcher("/(@[^/]*)"),
            "https://www.tiktok.com/oembed",
            ResourceType.Rich);

        // https://developers.tiktok.com/doc/embed-videos/
        AddScheme(
            new RegexMatcher(@"/(?:v|@[^\/]*\/video)\/(\d+)(?:\.html|(?:\?\S*)?)"),
            "https://www.tiktok.com/oembed",
            ResourceType.Video);
    }
}