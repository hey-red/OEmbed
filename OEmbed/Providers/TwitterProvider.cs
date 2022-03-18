using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record TwitterProvider : ProviderBase
    {
        public TwitterProvider()
        {
            AddAllowedHosts(new[]
            {
                "twitter.com",
                "www.twitter.com",
                "mobile.twitter.com"
            });

            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.|mobile\.)?twitter\.com/\S+/status(es)?/(\d+)(?:\?|/)?\S*"),
                apiEndpoint: "https://publish.twitter.com/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}