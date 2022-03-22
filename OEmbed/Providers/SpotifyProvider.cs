using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record SpotifyProvider : ProviderBase
    {
        public SpotifyProvider()
        {
            AddAllowedHosts(new[] { "open.spotify.com" });

            AddScheme(
                matcher: new RegexMatcher(
                    @"https?://open\.spotify\.com/
                    (?:artist|track|album|playlist|show|episode)/
                    ([a-zA-Z0-9]+)/?
                    (?:[^/^\s]*)?",

                    // https://open.spotify.com/user/{user_name}/playlist/{id}
                    @"https?://open\.spotify\.com/user/
                    (?:[a-zA-Z0-9]+)
                    /playlist/
                    ([a-zA-Z0-9]+)/?
                    (?:[^/^\s]*)?"),
                apiEndpoint: "https://open.spotify.com/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}