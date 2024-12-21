using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IOEmbedProvider
    {
        bool CanProcess(Uri uri);

        ResponseFormat Format { get; }

        ProviderScheme? MatchScheme(Uri uri);

        IEnumerable<KeyValuePair<string, string?>> Parameters { get; }

        Func<Uri, Uri>? PreProcessUrl { get; set; }
    }
}