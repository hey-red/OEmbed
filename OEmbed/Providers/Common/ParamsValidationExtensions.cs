using System;
using System.Collections.Generic;
using System.Linq;

namespace HeyRed.OEmbed.Providers.Common
{
    internal static class ParamsValidationExtensions
    {
        public static void ThrowIfInvalidMetaAccessToken(this IEnumerable<KeyValuePair<string, string?>>? parameters)
        {
            var tokenPair = parameters
                .EnsureNotNull()
                .First(p => p.Key == "access_token");

            if (tokenPair
                .Value
                .EnsureNotNullOrWhiteSpace()
                .Split('|').Length != 2)
            {
                throw new ArgumentException("The access_token should contains \"|\" separator.");
            }
        }
    }
}