using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

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
            new RegexMatcher(@"
                    /(?:(?:
                    gallery/(\w+)|
                    t/(?:\w+)/(\w+)|
                    a/(\w+)|
                    (\w+)
                    )?
                    (?:\.[a-zA-Z]{3,4})?)"),
            "https://api.imgur.com/oembed.json",
            ResourceType.Rich);
    }
}