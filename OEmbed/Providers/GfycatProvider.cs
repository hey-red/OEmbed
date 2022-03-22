using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record GfycatProvider : ProviderBase
    {
        private static readonly string[] _invalidPaths = new string[]
        {
            "api", "terms", "privacy", "about", "dmca", "search",
            "popular-gifs", "gaming", "stickers", "sound-gifs", "discover",
            "upload", "create", "login", "signup", "jobs", "partners", "blog",
            "faq", "support", "gifbrewery", "slack", "pro",
        };

        public GfycatProvider()
        {
            AddAllowedHosts(new[]
            {
                "gfycat.com",
                "thumbs.gfycat.com",
                "zippy.gfycat.com",
                "giant.gfycat.com"
            });

            // Direct links like https://giant.gfycat.com/RemoteIgnorantFallowdeer.mp4
            AddScheme(
                matcher: new RegexMatcher(@"
                    /([a-zA-Z0-9]+)
                    (?:\-mobile|\-size_restricted)?
                    \.(?:webm|mp4|gif)"),
                apiEndpoint: "https://api.gfycat.com/v1/oembed",
                resourceType: ResourceType.Video);

            // FIXME: lang code should be stripped from request url
            // https://gfycat.com/en/remoteignorantfallowdeer-kpop
            AddScheme(
                matcher: new RegexMatcher(@"
                    /(?:detail/|ifr/)?
                    (?!" + string.Join("|", _invalidPaths) + ")" + // Negative Lookahead
                    "([a-zA-Z0-9-]{3,})"),
                apiEndpoint: "https://api.gfycat.com/v1/oembed",
                resourceType: ResourceType.Video);
        }
    }
}