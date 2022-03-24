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

            // Statuses
            AddScheme(
                matcher: new RegexMatcher(@"/\w+/status(es)?/(\d+)(?:\?|/)?\S*"),
                apiEndpoint: "https://publish.twitter.com/oembed",
                resourceType: ResourceType.Rich);

            // Moments
            AddScheme(
                matcher: new RegexMatcher(@"/i/moments/(\d+)(?:\?|/)?\S*"),
                apiEndpoint: "https://publish.twitter.com/oembed?i=moment",
                resourceType: ResourceType.Rich);

            // Timelines
            AddScheme(
                matcher: new RegexMatcher(
                    @"/(\w+)/(?:timelines|lists)/(\d+)(?:\?|/)?\S*",
                    @"/(\w+)/likes(?:\?|/)?\S*"),
                apiEndpoint: "https://publish.twitter.com/oembed?i=timeline",
                resourceType: ResourceType.Rich);

            // Users
            AddScheme(
                matcher: new RegexMatcher(@"/(\w+)/?"),
                apiEndpoint: "https://publish.twitter.com/oembed?i=user",
                resourceType: ResourceType.Rich);
        }
    }
}