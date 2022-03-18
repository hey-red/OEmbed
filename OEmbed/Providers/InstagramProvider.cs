using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record InstagramProvider : ProviderBase
    {
        public InstagramProvider()
        {
            AddAllowedHosts(new[]
            {
                "instagram.com",
                "www.instagram.com",
                "instagr.am",
                "www.instagr.am"
            });

            AddScheme(
                matcher: new RegexMatcher(@"https?:\/\/(?:www\.)?instagr(?:\.am|am\.com)/p\/([^/?#&]+).*"),
                apiEndpoint: "http://api.instagram.com/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}