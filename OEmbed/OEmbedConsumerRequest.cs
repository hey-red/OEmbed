using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed;

public record OEmbedConsumerRequest : IOEmbedConsumerRequest
{
    public OEmbedConsumerRequest(
        Uri url,
        ResponseFormat? type = null,
        int? maxWidth = null,
        int? maxHeight = null)
    {
        Url = url.EnsureNotNull();
        Type = type;
        MaxWidth = maxWidth;
        MaxHeight = maxHeight;
    }

    /// <summary>
    ///     The required response format(XML/Json).
    /// </summary>
    public ResponseFormat? Type { get; }

    /// <summary>
    ///     The maximum width of the embedded resource.
    /// </summary>
    public int? MaxWidth { get; }

    /// <summary>
    ///     The maximum height of the embedded resource.
    /// </summary>
    public int? MaxHeight { get; }

    /// <summary>
    ///     The URL to retrieve embedding information for.
    /// </summary>
    public Uri Url { get; }

    public IEnumerable<KeyValuePair<string, string?>> BuildQueryParams()
    {
        var queryParams = new List<KeyValuePair<string, string?>>
        {
            new("url", Url.OriginalString)
        };

        if (MaxWidth is not null)
        {
            queryParams.Add(new KeyValuePair<string, string?>("maxwidth", MaxWidth.ToString()));
        }

        if (MaxHeight is not null)
        {
            queryParams.Add(new KeyValuePair<string, string?>("maxheight", MaxHeight.ToString()));
        }

        if (Type is not null)
        {
            queryParams.Add(new KeyValuePair<string, string?>("format", Type.ToString()!.ToLower()));
        }

        return queryParams;
    }
}