using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record SoundcloudProvider : ProviderBase
{
    public SoundcloudProvider()
    {
        AddAllowedHosts(new[] { "soundcloud.com", "on.soundcloud.com" });

        AddScheme(
            new RegexMatcher(@"/(?!(discover|stream|upload|popular|charts|people|pages|imprint|you)($|\/))([\S]+)"),
            "https://soundcloud.com/oembed",
            ResourceType.Rich);
    }
}