using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record KickstarterProvider : ProviderBase
    {
        public KickstarterProvider()
        {
            AddAllowedHosts(new[] { "www.kickstarter.com", "kickstarter.com" });

            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?kickstarter\.com/projects/(?:[a-z0-9-]+)/([a-z0-9-]+)(?:\?[^/^\s]*)?"),
                apiEndpoint: "https://www.kickstarter.com/services/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}