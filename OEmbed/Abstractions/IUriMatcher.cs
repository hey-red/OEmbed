using System;
using System.Collections.Generic;

namespace HeyRed.OEmbed.Abstractions;

public interface IUriMatcher
{
    bool IsMatch(Uri uri);

    UriMatch Match(Uri uri);
}

public record UriMatch(bool Success, IEnumerable<KeyValuePair<string, string>> CapturedValues);