using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record KickstarterProvider : ProviderBase
{
    public KickstarterProvider()
    {
        AddAllowedHosts(new[] { "www.kickstarter.com", "kickstarter.com" });

        AddScheme(
            new RegexMatcher(@"/projects/(?:[a-z0-9-]+)/([a-z0-9-]+)(?:\?[^/^\s]*)?"),
            "https://www.kickstarter.com/services/oembed",
            ResourceType.Rich);
    }
}