using System;
using System.Linq;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record TumblrProvider : ProviderBase
    {
        public TumblrProvider()
        {
            AddAllowedHosts(new[] { "tumblr.com" });

            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:[a-z0-9-]+\.)tumblr\.com/post/(\d+)/?(?:\S+)?"),
                apiEndpoint: "https://www.tumblr.com/oembed/1.0",
                resourceType: ResourceType.Rich);
        }

        public override bool CanProcess(Uri uri) => _allowedHosts.Any(host => uri.Host.Contains(host));
    }
}