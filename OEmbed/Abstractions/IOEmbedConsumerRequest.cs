using System;
using System.Collections.Generic;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IOEmbedConsumerRequest
    {
        Uri Url { get; }

        IEnumerable<KeyValuePair<string, string?>> BuildQueryParams();
    }
}