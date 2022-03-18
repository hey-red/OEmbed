using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record RedditProvider : ProviderBase
    {
        /// <summary>
        /// https://github.com/reddit-archive/reddit/wiki/oEmbed
        /// </summary>
        public RedditProvider(ProviderOptions? options = default)
        {
            AddParameters(options?.Parameters);

            AddAllowedHosts(new[]
            {
                "reddit.com",
                "www.reddit.com",
            });

            AddScheme(
                matcher: new RegexMatcher(@"
                    https?://(?:www\.)?(?:[\w-]+?\.)?reddit.com
                    (/r/|/user/)?(?(1)([\w:]{2,21}))(/comments/)?
                    (?(3)(\w{5,6})(?:/[\w%\\\\-]+)?)?(?(4)/(\w{7}))?/?(\?)?(?(6)(\S+))?(\#)?(?(8)(\S+))?"),
                apiEndpoint: "https://www.reddit.com/oembed",
                resourceType: ResourceType.Rich);
        }
    }
}