using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Abstractions;

public interface IOEmbedProvider
{
    ResponseFormat Format { get; }

    IEnumerable<KeyValuePair<string, string?>> Parameters { get; }

    Func<Uri, Uri>? PreProcessUrl { get; set; }
    bool CanProcess(Uri uri);

    ProviderScheme? MatchScheme(Uri uri);
}