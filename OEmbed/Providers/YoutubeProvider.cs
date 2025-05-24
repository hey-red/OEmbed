using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record YoutubeProvider : ProviderBase
{
    public YoutubeProvider(ProviderOptions? options = default)
    {
        // I can't find documentation about additional parameters..
        AddParameters(options?.Parameters);

        AddAllowedHosts(new[]
        {
            "m.youtube.com",
            "youtu.be",
            "youtube.com",
            "www.youtu.be",
            "www.youtube.com",
            "music.youtube.com"
        });

        AddScheme(
            new RegexMatcher(
                @"/(?:embed/|video/|shorts/|live/|playlist\?list=|watch\?v=)?([\w|-]+)(?:[\w\&\?\=\.\-]+)?"),
            "https://www.youtube.com/oembed",
            ResourceType.Video);
    }
}