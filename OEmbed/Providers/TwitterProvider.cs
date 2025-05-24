using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record TwitterProvider : ProviderBase
{
    /// <summary>
    ///     https://developer.twitter.com/en/docs/twitter-for-websites/oembed-api#Embedded
    /// </summary>
    /// <param name="parameters"></param>
    public TwitterProvider(ProviderOptions? options = default)
    {
        AddParameters(options?.Parameters);

        AddAllowedHosts([
            "twitter.com",
            "www.twitter.com",
            "mobile.twitter.com",
            "x.com"
        ]);

        // Statuses
        AddScheme(
            new RegexMatcher(@"/\w+/status(es)?/(\d+)(?:\?|/)?\S*"),
            "https://publish.twitter.com/oembed",
            ResourceType.Rich);

        // Moments
        AddScheme(
            new RegexMatcher(@"/i/moments/(\d+)(?:\?|/)?\S*"),
            "https://publish.twitter.com/oembed?i=moment",
            ResourceType.Rich);

        // Timelines
        AddScheme(
            new RegexMatcher(
                @"/(\w+)/(?:timelines|lists)/(\d+)(?:\?|/)?\S*",
                @"/(\w+)/likes(?:\?|/)?\S*"),
            "https://publish.twitter.com/oembed?i=timeline",
            ResourceType.Rich);

        // Users
        AddScheme(
            new RegexMatcher(@"/(\w+)/?"),
            "https://publish.twitter.com/oembed?i=user",
            ResourceType.Rich);
    }
}