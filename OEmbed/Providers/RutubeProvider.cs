using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record RutubeProvider : ProviderBase
    {
        public RutubeProvider()
        {
            AddAllowedHosts(new[]
            {
                "rutube.ru",
            });

            AddScheme(
                matcher: new RegexMatcher("/video/([a-zA-Z0-9]+)/?(?:.*)"),
                apiEndpoint: "https://rutube.ru/api/oembed/",
                resourceType: ResourceType.Video);
        }
    }
}
