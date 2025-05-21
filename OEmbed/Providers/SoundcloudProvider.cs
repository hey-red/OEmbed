using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record SoundcloudProvider : ProviderBase
    {
        public SoundcloudProvider()
        {
            AddAllowedHosts(new[] { "soundcloud.com", "on.soundcloud.com" });

            AddScheme(
                matcher: new RegexMatcher(@"/(?!discover|stream|upload|popular|charts|people|pages|imprint|you)([\S]+)"),
                apiEndpoint: "https://soundcloud.com/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}