using System;
using System.Text.RegularExpressions;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common
{
    public class RegexMatcher : IMatcher
    {
        private readonly Regex _matchRegex;

        public RegexMatcher(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            _matchRegex = new("^" + pattern + "$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        }

        public RegexMatcher(Regex regex)
        {
            _matchRegex = regex ?? throw new ArgumentNullException(nameof(regex));
        }

        public bool IsMatch(Uri uri) => _matchRegex.IsMatch(uri.OriginalString);
    }
}