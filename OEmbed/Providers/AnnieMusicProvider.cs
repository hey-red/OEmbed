using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record AnnieMusicProvider : ProviderBase
    {
        public AnnieMusicProvider()
        {
            AddAllowedHosts(new[] { "anniemusic.app" });

            AddScheme(
                matcher: new RegexMatcher(@"/(?:t|p)/([\w]+)([^/^\s]+)?"),
                apiEndpoint: "https://api.anniemusic.app/api/v1/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}