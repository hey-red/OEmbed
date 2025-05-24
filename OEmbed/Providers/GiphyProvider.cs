using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record GiphyProvider : ProviderBase
{
    public GiphyProvider()
    {
        AddAllowedHosts(new[]
        {
            "giphy.com",
            "www.giphy.com",
            "media.giphy.com"
        });

        // Gifs, stickers, media, embed
        AddScheme(
            new RegexMatcher(@"/(?:gifs|stickers|media|embed)/([\S]+)"),
            "https://giphy.com/services/oembed",
            ResourceType.Photo);

        // Clips
        AddScheme(
            new RegexMatcher(@"/clips/([\S]+)"),
            "https://giphy.com/services/oembed",
            ResourceType.Video);
    }
}