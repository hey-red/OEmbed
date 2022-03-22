using System.Linq;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    /// <summary>
    /// You should provide "access_token" parameter, which value is app_id|token
    /// More info https://developers.facebook.com/docs/development/create-an-app
    /// Without token the legacy scheme is used(limited to post url).
    /// </summary>
    public record InstagramProvider : ProviderBase
    {
        public InstagramProvider(ProviderOptions? options = default)
        {
            AddParameters(options?.Parameters);

            AddAllowedHosts(new[]
            {
                "instagram.com",
                "www.instagram.com",
                "instagr.am",
                "www.instagr.am"
            });

            // Actual endpoint https://developers.facebook.com/docs/graph-api/reference/instagram-oembed/
            if (options?.Parameters is not null &&
                options.Parameters.Any(p => p.Key == "access_token"))
            {
                // I think validation is better than error in runtime
                options.Parameters.ThrowIfInvalidMetaAccessToken();

                // Posts, reels, tv
                AddScheme(
                    matcher: new RegexMatcher(@"
                        /(?:[a-zA-Z0-9_\-\.]+/)?
                        (?:p|tv|reel)/([a-zA-Z0-9_-]+)/?
                        (?:[\w?#&=]+)?"),
                    apiEndpoint: "https://graph.facebook.com/v13.0/instagram_oembed",
                    resourceType: ResourceType.Rich);
            }
            // Legacy endpoint https://developers.facebook.com/docs/instagram/oembed-legacy
            else
            {
                // NOTE: limited to post
                AddScheme(
                    matcher: new RegexMatcher(@"/p\/([^/?#&]+).*"),
                    apiEndpoint: "http://api.instagram.com/oembed",
                    resourceType: ResourceType.Rich);
            }
        }
    }
}