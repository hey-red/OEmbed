using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed
{
    public record OEmbedProviderInfo(
        ProviderScheme Scheme,
        ResponseFormat ResponseFormat,
        IEnumerable<KeyValuePair<string, string?>> Parameters,
        Func<Uri, Uri>? UrlPreProcessor);
}