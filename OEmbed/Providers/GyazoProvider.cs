using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    /// <summary>
    /// https://gyazo.com/api/docs/image#oembed
    /// </summary>
    public record GyazoProvider : ProviderBase
    {
        public GyazoProvider()
        {
            AddAllowedHosts(new[] { "gyazo.com", "www.gyazo.com" });

            AddScheme(
                matcher: new RegexMatcher(@"/(\S{32,40})"),
                apiEndpoint: "https://api.gyazo.com/api/oembed",
                resourceType: ResourceType.Photo);
        }
    }
}