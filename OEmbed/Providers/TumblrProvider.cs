using System;
using System.Linq;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record TumblrProvider : ProviderBase
{
    public TumblrProvider()
    {
        AddAllowedHosts(new[] { "tumblr.com" });

        AddScheme(
            new RegexMatcher(@"/post/(\d+)/?(?:\S+)?"),
            "https://www.tumblr.com/oembed/1.0",
            ResourceType.Rich);
    }

    public override bool CanProcess(Uri uri)
    {
        return _allowedHosts.Any(host => uri.Host.Contains(host));
    }
}