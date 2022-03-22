using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record VimeoProvider : ProviderBase
    {
        /// <summary>
        /// https://developer.vimeo.com/api/oembed/videos
        /// </summary>
        public VimeoProvider(ProviderOptions? options = default)
        {
            AddParameters(options?.Parameters);

            AddAllowedHosts(new[]
            {
                "vimeo.com",
                "www.vimeo.com",
                "player.vimeo.com"
            });

            // Regular video, channels, groups, ondemand
            AddScheme(
                matcher: new RegexMatcher(
                    @"/(?:channels\/(?:\w+\/)|groups\/(?:[^\/]*\/videos)\/|ondemand(.+)|)
                    (\d+)?(?:|\/\?)(?:\?\S+)?"),
                apiEndpoint: "https://vimeo.com/api/oembed.json",
                resourceType: ResourceType.Video);

            // https://vimeo.com/terjes/themountain
            // https://vimeo.com/terjes/themountain#t=5s
            AddScheme(
                matcher: new RegexMatcher(@"/(?!ondemand)\w+\/(\w+)(?:(?:\?|\#)\S+)?"),
                apiEndpoint: "https://vimeo.com/api/oembed.json",
                resourceType: ResourceType.Video);
        }
    }
}