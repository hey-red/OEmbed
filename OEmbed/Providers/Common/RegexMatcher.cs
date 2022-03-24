using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common
{
    public class RegexMatcher : IUriMatcher
    {
        private readonly List<Regex> _expressions = new();

        public RegexMatcher(string pattern) : this(new string[] { pattern })
        {
        }

        public RegexMatcher(params string[] patterns)
        {
            patterns.EnsureNotNull();

            foreach (var pattern in patterns)
            {
                pattern.EnsureNotNullOrWhiteSpace();

                _expressions.Add(new("^" + pattern + "$", RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled));
            }
        }

        public RegexMatcher(Regex regex) : this(new Regex[] { regex.EnsureNotNull() })
        {
        }

        public RegexMatcher(params Regex[] expressions) => _expressions.AddRange(expressions.EnsureNotNull());

        public bool IsMatch(Uri uri) => _expressions.Any(x => x.IsMatch(uri.PathAndQuery));

        public UriMatch Match(Uri uri)
        {
            foreach (var re in _expressions)
            {
                var match = re.Match(uri.PathAndQuery);
                if (match.Success)
                {
                    var values = new List<KeyValuePair<string, string>>();

                    foreach (var key in match.Groups.Keys.Skip(1))
                    {
                        values.Add(new(key, match.Groups[key].Value));
                    }

                    return new(true, values);
                }
            }

            return new(false, Array.Empty<KeyValuePair<string, string>>());
        }
    }
}