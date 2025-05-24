using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record PixivProvider : ProviderBase
{
    public PixivProvider()
    {
        AddAllowedHosts(new[] { "pixiv.net", "www.pixiv.net" });

        AddScheme(
            new RegexMatcher(
                /*
                 * Old urls format
                 * https://www.pixiv.net/member_illust.php?mode=medium&illust_id={id}
                 */
                @"/(?:[?&=\._a-z0-9]+)illust_id=([0-9]+)",
                /*
                 * https://www.pixiv.net/en/artworks/{id}
                 * https://www.pixiv.net/artworks/{id}
                 */
                @"/(?:en/)?artworks/([0-9]+)"),
            "https://embed.pixiv.net/oembed.php",
            ResourceType.Rich);
    }
}