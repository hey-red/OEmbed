using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record GfycatProvider : ProviderBase
    {
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
                matcher: new RegexMatcher(@"https?://(?:giant|thumbs|zippy)\.gfycat\.com/([a-zA-Z0-9]+)(?:\-mobile|\-size_restricted)?\.(?:webm|mp4|gif)"),
                apiEndpoint: "https://api.gfycat.com/v1/oembed",
                resourceType: ResourceType.Video);

            // FIXME: lang code should be stripped from request url
            // https://gfycat.com/en/remoteignorantfallowdeer-kpop
            AddScheme(
                matcher: new RegexMatcher(@"https?://gfycat\.com\/(?:detail/|ifr/|[a-z]{2}/)?(\S+)"),
                apiEndpoint: "https://api.gfycat.com/v1/oembed",
                resourceType: ResourceType.Video);
        }
    }
}