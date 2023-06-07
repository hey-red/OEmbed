using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record AudioboomProvider : ProviderBase
    {
        public AudioboomProvider()
        {
            AddAllowedHosts(new[] { "audioboom.com" });

            AddScheme(
                matcher: new RegexMatcher(@"/(?:channel|posts)/([\w-]+)/?"),
                apiEndpoint: "https://audioboom.com/publishing/oembed.json",
                resourceType: ResourceType.Rich);
        }
    }
}