using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common
{
    public class RegexMatcher : IUriMatcher
    {
        private readonly List<Regex> _matchRegex = new();

        public RegexMatcher(string pattern) : this(new string[] { pattern })
        {
        }

        public RegexMatcher(params string[] patterns)
        {
            patterns.EnsureNotNull();

            foreach (var pattern in patterns)
            {
                pattern.EnsureNotNullOrWhiteSpace();

                _matchRegex.Add(new("^" + pattern + "$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled));
            }
        }

        public RegexMatcher(Regex regex) : this(new Regex[] { regex.EnsureNotNull() })
        {
        }

        public RegexMatcher(params Regex[] expressions) => _matchRegex.AddRange(expressions.EnsureNotNull());

        public bool IsMatch(Uri uri) => _matchRegex.Any(re => re.IsMatch(uri.OriginalString));
    }
}