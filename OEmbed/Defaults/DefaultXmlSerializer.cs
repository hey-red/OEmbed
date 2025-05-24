using System;
using System.Buffers;
using System.Globalization;
using System.IO;
using System.Reflection;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Models;

using U8Xml;

namespace HeyRed.OEmbed.Defaults;

public class DefaultXmlSerializer : IXmlSerializer
{
    public T? Deserialize<T>(Stream content) where T : Base
    {
        using XmlObject doc = XmlParser.Parse(content);
        XmlNode root = doc.Root;

        if (root.Name != "oembed")
        {
            throw new InvalidDataException("The root element should be called as \"oembed\". Value: " + doc.Root.Name);
        }

        Type type = typeof(T);
        object? obj = Activator.CreateInstance(type);

        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        foreach (PropertyInfo property in properties)
        {
            if (!property.CanWrite)
            {
                continue;
            }

            if (root.TryFindChild(NameConverter.ConvertName(property.Name), out XmlNode node))
            {
                if ((IsNullableType(property.PropertyType) &&
                     Nullable.GetUnderlyingType(property.PropertyType) == typeof(int)) ||
                    property.PropertyType == typeof(int))
                {
                    property.SetValue(obj, node.InnerText.ToInt32());
                }
                else
                {
                    property.SetValue(obj, node.InnerText.ToString());
                }
            }
        }

        return (T?)obj;
    }

    private static bool IsNullableType(Type type)
    {
        return type.IsGenericType &&
               type
                   .GetGenericTypeDefinition()
                   .Equals(typeof(Nullable<>));
    }
}

/// <summary>
///     Taken from https://github.com/YohDeadfall/Yoh.Text.Json.NamingPolicies/blob/master/src/JsonSimpleNamingPolicy.cs
/// </summary>
internal static class NameConverter
{
    private const char BOUNDARY = '_';

    public static string ConvertName(string name)
    {
        int bufferLength = name.Length * 2;
        char[]? buffer = bufferLength > 512
            ? ArrayPool<char>.Shared.Rent(bufferLength)
            : null;

        var resultLength = 0;
        var result = buffer is null
            ? stackalloc char[512]
            : buffer;

        void WriteWord(ref Span<char> result, ReadOnlySpan<char> word)
        {
            if (word.IsEmpty)
            {
                return;
            }

            int required = result.IsEmpty
                ? word.Length
                : word.Length + 1;

            if (required >= result.Length)
            {
                int bufferLength = result.Length * 2;
                char[] bufferNew = ArrayPool<char>.Shared.Rent(bufferLength);

                result.CopyTo(bufferNew);

                if (buffer is not null)
                {
                    ArrayPool<char>.Shared.Return(buffer);
                }

                buffer = bufferNew;
            }

            if (resultLength != 0)
            {
                result[resultLength] = BOUNDARY;
                resultLength += 1;
            }

            var destination = result[resultLength..];

            word.ToLowerInvariant(destination);

            resultLength += word.Length;
        }

        var first = 0;
        var chars = name.AsSpan();
        var previousCategory = CharCategory.Boundary;
        for (var index = 0; index < chars.Length; index++)
        {
            char current = chars[index];
            UnicodeCategory currentCategoryUnicode = char.GetUnicodeCategory(current);
            if (currentCategoryUnicode == UnicodeCategory.SpaceSeparator ||
                (currentCategoryUnicode >= UnicodeCategory.ConnectorPunctuation &&
                 currentCategoryUnicode <= UnicodeCategory.OtherPunctuation))
            {
                WriteWord(ref result, chars[first..index]);

                previousCategory = CharCategory.Boundary;
                first = index + 1;

                continue;
            }

            if (index + 1 < chars.Length)
            {
                char next = chars[index + 1];
                CharCategory currentCategory = currentCategoryUnicode switch
                {
                    UnicodeCategory.LowercaseLetter => CharCategory.Lowercase,
                    UnicodeCategory.UppercaseLetter => CharCategory.Uppercase,
                    _ => previousCategory
                };

                if ((currentCategory == CharCategory.Lowercase && char.IsUpper(next)) ||
                    next == '_')
                {
                    WriteWord(ref result, chars[first..(index + 1)]);

                    previousCategory = CharCategory.Boundary;
                    first = index + 1;

                    continue;
                }

                if (previousCategory == CharCategory.Uppercase &&
                    currentCategoryUnicode == UnicodeCategory.UppercaseLetter &&
                    char.IsLower(next))
                {
                    WriteWord(ref result, chars[first..index]);

                    previousCategory = CharCategory.Boundary;
                    first = index;

                    continue;
                }

                previousCategory = currentCategory;
            }
        }

        WriteWord(ref result, chars[first..]);

        name = new string(result[..resultLength]);

        if (buffer is not null)
        {
            ArrayPool<char>.Shared.Return(buffer);
        }

        return name;
    }

    private enum CharCategory
    {
        Boundary,
        Lowercase,
        Uppercase
    }
}