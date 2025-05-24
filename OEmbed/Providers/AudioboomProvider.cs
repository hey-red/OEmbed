using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record AudioboomProvider : ProviderBase
{
    public AudioboomProvider()
    {
        AddAllowedHosts(new[] { "audioboom.com" });

        AddScheme(
            new RegexMatcher(@"/(?:channel|posts)/([\w-]+)/?"),
            "https://audioboom.com/publishing/oembed.json",
            ResourceType.Rich);
    }
}