using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using HeyRed.OEmbed.Providers;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IOEmbedProvider
    {
        bool CanProcess(Uri uri);

        ResponseFormat ResponseType { get; }

        IDictionary<Regex, ProviderScheme> Schemes { get; }
    }
}