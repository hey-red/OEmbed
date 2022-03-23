using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record CodepenProvider : ProviderBase
    {
        public CodepenProvider()
        {
            AddAllowedHosts(new[] { "codepen.io" });

            AddScheme(
                matcher: new RegexMatcher(@"/(?:team/)?(?:[\w]+)/pen/([\w]+)/?"),
                apiEndpoint: "https://codepen.io/api/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}