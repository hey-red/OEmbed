using System;

namespace HeyRed.OEmbed.Defaults
{
    public class CacheOptions
    {
        public DateTimeOffset AbsoluteExpiration { get; set; } = DateTimeOffset.UtcNow.AddHours(1);
    }
}