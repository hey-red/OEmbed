﻿using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    public record CoubProvider : ProviderBase
    {
        public CoubProvider()
        {
            AddAllowedHosts(new[] { "coub.com" });

            AddScheme(
                matcher: new RegexMatcher(@"https?://(?:www\.)?coub\.com/(?:view|embed)/(\w+)"),
                apiEndpoint: "https://coub.com/api/oembed.json",
                resourceType: ResourceType.Video);
        }
    }
}