using System;
using System.Collections.Generic;
using System.Linq;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Providers
{
    /// <summary>
    /// What about reading the spec, yandex team? 🤦‍♀️
    /// NOTE: Can response with captcha type. Rate limit?
    /// </summary>
    public record YandexMusicProvider : ProviderBase
    {
        public YandexMusicProvider()
        {
            AddAllowedHosts(new[] { "music.yandex.ru" });

            AddScheme(
                matcher: new RegexMatcher(
                    @"/(?:
                    album/(?<albumId>\d+)|
                    album/(?<albumId>\d+)/track/(?<trackId>\d+)|
                    track/(?<trackId>\d+)
                    )/?"),
                apiEndpoint: "https://music.yandex.ru/handlers/oembed-json.jsx",
                resourceType: ResourceType.Rich);
        }

        public override ProviderScheme? MatchScheme(Uri uri)
        {
            foreach (var scheme in _schemes)
            {
                UriMatch match = scheme.Key.Match(uri);
                if (match.Success)
                {
                    var queryPairs = new List<KeyValuePair<string, string?>>();
                    var endpoint = scheme.Value.Endpoint.ToString();

                    // Albums, tracks
                    var albumId = match.CapturedValues
                        .Where(v => v.Key == "albumId")
                        .Select(v => v.Value)
                        .FirstOrDefault();

                    var trackId = match.CapturedValues
                        .Where(v => v.Key == "trackId")
                        .Select(v => v.Value)
                        .FirstOrDefault();

                    if (!string.IsNullOrWhiteSpace(albumId))
                    {
                        queryPairs.Add(new("album", albumId));
                    }

                    if (!string.IsNullOrWhiteSpace(trackId))
                    {
                        queryPairs.Add(new("track", trackId));
                    }

                    if (queryPairs.Any())
                    {
                        endpoint = UrlHelpers.AddQueryString(endpoint, queryPairs);
                    }

                    return new ProviderScheme(new Uri(endpoint), scheme.Value.ResourceType);
                }
            }

            return null;
        }
    }
}