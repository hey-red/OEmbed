using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record VimeoProvider : ProviderBase
{
    /// <summary>
    ///     https://developer.vimeo.com/api/oembed/videos
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
            new RegexMatcher(
                @"/(?:channels\/(?:\w+\/)|groups\/(?:[^\/]*\/videos)\/|ondemand(.+)|)
                    (\d+)?(?:|\/\?)(?:\?\S+)?"),
            "https://vimeo.com/api/oembed.json",
            ResourceType.Video);

        // https://vimeo.com/terjes/themountain
        // https://vimeo.com/terjes/themountain#t=5s
        AddScheme(
            new RegexMatcher(@"/(?!ondemand)\w+\/(\w+)(?:(?:\?|\#)\S+)?"),
            "https://vimeo.com/api/oembed.json",
            ResourceType.Video);
    }
}