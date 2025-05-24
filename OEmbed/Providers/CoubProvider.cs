using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record CoubProvider : ProviderBase
{
    public CoubProvider()
    {
        AddAllowedHosts(new[] { "coub.com" });

        AddScheme(
            new RegexMatcher(@"/(?:view|embed)/(\w+)"),
            "https://coub.com/api/oembed.json",
            ResourceType.Video);
    }
}