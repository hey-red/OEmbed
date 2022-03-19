using System;
using System.Text.RegularExpressions;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common
{
    public class RegexMatcher : IUriMatcher
    {
        private readonly Regex _matchRegex;

        public RegexMatcher(string pattern)
        {
            pattern.EnsureNotNullOrWhiteSpace();

            _matchRegex = new("^" + pattern + "$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
        }

        public RegexMatcher(Regex regex) => _matchRegex = regex.EnsureNotNull();

        public bool IsMatch(Uri uri) => _matchRegex.IsMatch(uri.OriginalString);
    }
}