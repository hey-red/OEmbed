using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record ImgurProvider : ProviderBase
    {
        public ImgurProvider()
        {
            AddAllowedHosts(new[]
            {
                "imgur.com",
                "i.imgur.com",
                // Partners
                "i.stack.imgur.com"
            });

            AddScheme(
                matcher: new RegexMatcher(@"
                    https?://(?:(?:i\.[a-z]*\.)|i\.)?
                    imgur\.com/
                    (?:(?:
                    gallery/(\w+)|
                    t/(?:\w+)/(\w+)|
                    a/(\w+)|
                    (\w+)
                    )?
                    (?:\.[a-zA-Z]{3,4})?)"),
                apiEndpoint: "https://api.imgur.com/oembed.json",
                resourceType: ResourceType.Rich);
        }
    }
}