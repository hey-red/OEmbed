using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    /// <summary>
    /// https://developers.facebook.com/docs/instagram/oembed-legacy
    /// TODO: migrate to new endpoints.
    /// </summary>
    public record InstagramProvider : ProviderBase
    {
        public InstagramProvider(ProviderOptions? options = default)
        {
            AddParameters(options?.Parameters);

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