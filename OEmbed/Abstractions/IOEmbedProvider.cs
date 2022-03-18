using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IOEmbedProvider
    {
        bool CanProcess(Uri uri);

        ResponseFormat ResponseType { get; }

        IReadOnlyDictionary<IMatcher, ProviderScheme> Schemes { get; }

        IEnumerable<KeyValuePair<string, string?>> Parameters { get; }
    }
}