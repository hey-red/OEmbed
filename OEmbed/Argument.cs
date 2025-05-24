using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace HeyRed.OEmbed;

public static class Argument
{
    public static T EnsureNotNull<T>([NotNull] this T? value, [CallerArgumentExpression("value")] string name = "")
        where T : class
    {
        return value is null ? throw new ArgumentNullException(name) : value;
    }

    public static string EnsureNotNullOrWhiteSpace(
        [NotNull] this string? value,
        [CallerArgumentExpression("value")] string name = "")
    {
        return string.IsNullOrWhiteSpace(value)
            ? throw new ArgumentException("String cannot be empty", name)
            : value;
    }
}