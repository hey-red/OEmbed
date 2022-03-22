using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record FlickrProvider : ProviderBase
    {
        public FlickrProvider()
        {
            AddAllowedHosts(new[]
            {
                "www.flickr.com",
                "flickr.com",
                "flic.kr"
            });

            // Photos
            AddScheme(
                matcher: new RegexMatcher(
                    @"https?://(?:www\.)?flickr\.com/photos/(?:[@a-zA-Z0-9_\.\-]+)/([0-9]+)(?:/in/[^/\s]+)?/?/?",
                    @"https?://flic\.kr/p/([a-zA-Z0-9]+)"),
                apiEndpoint: "https://www.flickr.com/services/oembed",
                resourceType: ResourceType.Photo);

            // Albums(also known as sets), photostream, favorites
            // NOTE: Short url for albums is not work with oembed endpoint
            AddScheme(
                matcher: new RegexMatcher(
                    @"https?://(?:www\.)?flickr\.com/photos/([@a-zA-Z0-9_\.\-]+)(?:/(?:favorites|(?:albums|sets)/([0-9]+)))?/?"),
                apiEndpoint: "https://www.flickr.com/services/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}