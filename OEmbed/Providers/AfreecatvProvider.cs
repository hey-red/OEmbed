using System.Text.RegularExpressions;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers;

public record AfreecatvProvider : ProviderBase
{
    public AfreecatvProvider()
    {
        AddAllowedHosts(new[]
        {
            "vod.afreecatv.com",
            "play.afreecatv.com",
            "vod.afree.ca",
            "play.afree.ca",
            "vod.sooplive.co.kr",
            "play.sooplive.co.kr"
        });

        AddScheme(
            new RegexMatcher(
                new Regex(@"/(?:player(?:/station)?|[a-z0-9]+)/([0-9])+/?",
                    RegexOptions.Compiled |
                    RegexOptions.IgnoreCase)),
            "https://openapi.afreecatv.com/vod/embedinfo",
            ResourceType.Video);
    }
}