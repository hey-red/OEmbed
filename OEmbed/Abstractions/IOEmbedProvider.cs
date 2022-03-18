using System;
using System.Collections.Generic;

using HeyRed.OEmbed.Providers.Common;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IOEmbedProvider
    {
        bool CanProcess(Uri uri);

        ResponseFormat ResponseType { get; }

        IDictionary<IMatcher, ProviderScheme> Schemes { get; }
    }
}