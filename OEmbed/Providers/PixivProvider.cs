using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record PixivProvider : ProviderBase
    {
        public PixivProvider()
        {
            AddAllowedHosts(new[] { "pixiv.net", "www.pixiv.net" });

            AddScheme(
                matcher: new RegexMatcher(
                    /*
                     * Old urls format
                     * https://www.pixiv.net/member_illust.php?mode=medium&illust_id={id}
                     */
                    @"https?://(?:www\.)?pixiv\.net/(?:[?&=\._a-z0-9]+)illust_id=([0-9]+)",

                    /*
                     * https://www.pixiv.net/en/artworks/{id}
                     * https://www.pixiv.net/artworks/{id}
                     */
                    @"https?://(?:www\.)?pixiv\.net/(?:en/)?artworks/([0-9]+)"),
                apiEndpoint: "https://embed.pixiv.net/oembed.php",
                resourceType: ResourceType.Rich);
        }
    }
}