using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using HeyRed.OEmbed.Abstractions;

namespace HeyRed.OEmbed.Providers.Common;

public class RegexMatcher : IUriMatcher
{
    private readonly List<Regex> _expressions = new();

    public RegexMatcher(string pattern) : this(new[] { pattern })
    {
    }

    public RegexMatcher(params string[] patterns)
    {
        patterns.EnsureNotNull();

        foreach (string pattern in patterns)
        {
            pattern.EnsureNotNullOrWhiteSpace();

            _expressions.Add(new Regex("^" + pattern + "$",
                RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled));
        }
    }

    public RegexMatcher(Regex regex) : this(new[] { regex.EnsureNotNull() })
    {
    }

    public RegexMatcher(params Regex[] expressions)
    {
        _expressions.AddRange(expressions.EnsureNotNull());
    }

    public bool IsMatch(Uri uri)
    {
        return _expressions.Any(x => x.IsMatch(uri.PathAndQuery));
    }

    public UriMatch Match(Uri uri)
    {
        foreach (Regex re in _expressions)
        {
            Match match = re.Match(uri.PathAndQuery);
            if (match.Success)
            {
                var values = new List<KeyValuePair<string, string>>();

                foreach (string key in match.Groups.Keys.Skip(1))
                {
                    values.Add(new KeyValuePair<string, string>(key, match.Groups[key].Value));
                }

                return new UriMatch(true, values);
            }
        }

        return new UriMatch(false, Array.Empty<KeyValuePair<string, string>>());
    }
}