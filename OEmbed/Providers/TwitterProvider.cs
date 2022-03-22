using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record TwitterProvider : ProviderBase
    {
        /// <summary>
        /// https://developer.twitter.com/en/docs/twitter-for-websites/oembed-api#Embedded
        /// </summary>
        /// <param name="parameters"></param>
        public TwitterProvider(ProviderOptions? options = default)
        {
            AddParameters(options?.Parameters);

            AddAllowedHosts(new[]
            {
                "twitter.com",
                "www.twitter.com",
                "mobile.twitter.com"
            });

            AddScheme(
                matcher: new RegexMatcher(@"/\S+/status(es)?/(\d+)(?:\?|/)?\S*"),
                apiEndpoint: "https://publish.twitter.com/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}