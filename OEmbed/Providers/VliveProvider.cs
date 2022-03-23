using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record VliveProvider : ProviderBase
    {
        public VliveProvider()
        {
            AddAllowedHosts(new[] { "www.vlive.tv", "vlive.tv" });

            AddScheme(
                matcher: new RegexMatcher(@"/video/([0-9]+)"),
                apiEndpoint: "https://www.vlive.tv/oembed",
                resourceType: ResourceType.Video);
        }
    }
}