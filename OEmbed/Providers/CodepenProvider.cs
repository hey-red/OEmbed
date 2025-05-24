using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record CodepenProvider : ProviderBase
{
    public CodepenProvider()
    {
        AddAllowedHosts(new[] { "codepen.io" });

        AddScheme(
            new RegexMatcher(@"/(?:team/)?(?:[\w]+)/pen/([\w]+)/?"),
            "https://codepen.io/api/oembed",
            ResourceType.Rich);
    }
}