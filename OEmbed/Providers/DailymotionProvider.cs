using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record DailymotionProvider : ProviderBase
    {
        public DailymotionProvider()
        {
            AddAllowedHosts(new[] { "dailymotion.com", "www.dailymotion.com", "dai.ly" });

            AddScheme(
                matcher: new RegexMatcher(@"/(?:(?:embed/)?video/)?([\w]+)(?:[\w\?=]+)?"),
                apiEndpoint: "https://www.dailymotion.com/services/oembed",
                resourceType: ResourceType.Video);
        }
    }
}