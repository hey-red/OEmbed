using System;

namespace HeyRed.OEmbed.Defaults
{
    public class CacheOptions
    {
        public DateTimeOffset AbsoluteExpiration { get; } = DateTimeOffset.UtcNow.AddHours(1);
    }
}