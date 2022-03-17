using System;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IOEmbedConsumerRequest
    {
        Uri Url { get; }
    }
}