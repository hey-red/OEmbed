using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    /// <summary>
    /// TODO: pin.it is unsupported by oEmbed endpoint
    /// </summary>
    public record PinterestProvider : ProviderBase
    {
        public PinterestProvider()
        {
            AddAllowedHosts(new[] { "www.pinterest.com", "pinterest.com" });

            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?(?:pinterest\.[a-z]+/pin/([\d]+)/?)"),
                apiEndpoint: "https://www.pinterest.com/oembed.json",
                resourceType: ResourceType.Rich);
        }
    }
}