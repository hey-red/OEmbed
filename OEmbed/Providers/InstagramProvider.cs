using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    /// <summary>
    /// You should provide "access_token" parameter, which value is app_id|token
    /// More info https://developers.facebook.com/docs/development/create-an-app
    /// </summary>
    public record InstagramProvider : ProviderBase
    {
        public InstagramProvider(ProviderOptions options)
        {
            options.Parameters.ThrowIfInvalidMetaAccessToken();

            AddParameters(options.Parameters);

            AddAllowedHosts(new[]
            {
                "instagram.com",
                "www.instagram.com",
                "instagr.am",
                "www.instagr.am"
            });

            // Actual endpoint https://developers.facebook.com/docs/graph-api/reference/instagram-oembed/
            // Posts, reels, tv, user
            AddScheme(
                matcher: new RegexMatcher(@"
                        /(?:[\w_\-]+/)?
                        (?:p/|tv/|reel/)?([a-zA-Z0-9_-]+)/?
                        (?:[\w?#&=]+)?"),
                apiEndpoint: "https://graph.facebook.com/v18.0/instagram_oembed",
                resourceType: ResourceType.Rich);
        }
    }
}