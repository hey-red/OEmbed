using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record TiktokProvider : ProviderBase
    {
        /// <summary>
        /// https://developers.tiktok.com/doc/embed-videos
        /// </summary>
        public TiktokProvider()
        {
            AddAllowedHosts(new[]
            {
                "tiktok.com",
                "www.tiktok.com",
                "m.tiktok.com"
            });
            
            // https://www.tiktok.com/*
            AddScheme(
                matcher: new RegexMatcher(@"^/([^/]*)"),
                apiEndpoint: "https://www.tiktok.com/oembed",
                resourceType: ResourceType.Rich);

            // https://www.tiktok.com/*/video/*
            AddScheme(
                matcher: new RegexMatcher(@"^/(\S*)/video/(\S*)"),
                apiEndpoint: "https://www.tiktok.com/oembed",
                resourceType: ResourceType.Video);
            
            // https://www.tiktok.com/v/*.html
            AddScheme(
                matcher: new RegexMatcher(@"^/v/(\S*)\.html"),
                apiEndpoint: "https://www.tiktok.com/oembed",
                resourceType: ResourceType.Video);
        }
    }
}