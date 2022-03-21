using System;
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
        private const string TOKEN_PARAM_KEY = "access_token";

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
                options.Parameters.Any(p => p.Key == TOKEN_PARAM_KEY))
            {
                // I think validation is better than error in runtime
                string? token = options.Parameters.First(p => p.Key == TOKEN_PARAM_KEY).Value;

                if (token
                    .EnsureNotNullOrWhiteSpace()
                    .Split('|').Length != 2)
                {
                    throw new ArgumentException("The access_token should contains \"|\" separator.");
                }

                // Posts, reels, tv
                AddScheme(
                    matcher: new RegexMatcher(@"
                        https?://(?:www\.)?instagr(?:\.am|am\.com)/
                        (?:[a-zA-Z0-9_\-\.]+/)?
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
                    matcher: new RegexMatcher(@"https?://(?:www\.)?instagr(?:\.am|am\.com)/p\/([^/?#&]+).*"),
                    apiEndpoint: "http://api.instagram.com/oembed",
                    resourceType: ResourceType.Rich);
            }
        }
    }
}