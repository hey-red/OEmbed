using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    /// <summary>
    /// https://www.deviantart.com/developers/oembed
    /// </summary>
    public record DeviantartProvider : ProviderBase
    {
        public DeviantartProvider()
        {
            AddAllowedHosts(new[]
            {
                "www.deviantart.com",
                "deviantart.com",
                "fav.me",
                "sta.sh"
            });

            AddScheme(
                matcher: new RegexMatcher(
                    /*
                     * https://www.deviantart.com/{author_name}/art/{id}
                     * https://www.deviantart.com/art/{id}
                     */
                    @"/(?:[a-z0-9-_]+/)?art/([\S]+)/?",

                    /*
                     * https://fav.me/{id}
                     * https://sta.sh/{id}
                     */
                    @"/([a-zA-Z0-9]+)/?"),
                apiEndpoint: "https://backend.deviantart.com/oembed",
                resourceType: ResourceType.Photo);
        }
    }
}