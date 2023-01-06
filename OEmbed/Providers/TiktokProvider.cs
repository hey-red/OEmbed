using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
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

            // https://developers.tiktok.com/doc/embed-videos/
            AddScheme(
                matcher: new RegexMatcher(@"/(?:v|@[^\/]*\/video)\/(\d+)(?:\.html|(?:\?\S*)?)"),
                apiEndpoint: "https://www.tiktok.com/oembed",
                resourceType: ResourceType.Video);
        }
    }
}