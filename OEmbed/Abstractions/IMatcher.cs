using System;

namespace HeyRed.OEmbed.Abstractions
{
    public interface IMatcher
    {
        bool IsMatch(Uri uri);
    }
}