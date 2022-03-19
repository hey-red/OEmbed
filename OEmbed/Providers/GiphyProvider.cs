using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record GiphyProvider : ProviderBase
    {
        public GiphyProvider()
        {
            AddAllowedHosts(new[]
            {
                "giphy.com",
                "www.giphy.com",
                "media.giphy.com",
            });

            // Gifs, stickers, media, embed
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.|media\.)?giphy\.com/(?:gifs|stickers|media|embed)/([\S]+)"),
                apiEndpoint: "https://giphy.com/services/oembed",
                resourceType: ResourceType.Photo);

            // Clips
            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?giphy\.com/clips/([\S]+)"),
                apiEndpoint: "https://giphy.com/services/oembed",
                resourceType: ResourceType.Video);
        }
    }
}