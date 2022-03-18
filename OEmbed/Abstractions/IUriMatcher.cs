using System;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IUriMatcher
    {
        bool IsMatch(Uri uri);
    }
}