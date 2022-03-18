using System.Collections.Generic;

namespace HeyRed.OEmbed.Providers.Common
{
    public class ProviderOptions
    {
        public IEnumerable<KeyValuePair<string, string?>>? Parameters { get; set; } = default;
    }
}