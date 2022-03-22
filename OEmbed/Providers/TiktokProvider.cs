using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record TiktokProvider : ProviderBase
    {
        /// <summary>
        /// https://developers.tiktok.com/doc/embed-videos
        /// </summary>
        public TiktokProvider()
        {
            AddAllowedHosts(new[]
            {
                "tiktok.com",
                "www.tiktok.com",
                "m.tiktok.com"
            });

            AddScheme(
                matcher: new RegexMatcher(@"/(?:@\S+)?(?:v|video)/(\d+)(?:html|\S*)"),
                apiEndpoint: "https://www.tiktok.com/oembed",
                resourceType: ResourceType.Video);
        }
    }
}