using System;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Models;

namespace HeyRed.OEmbed.Defaults;

public class DefaultJsonSerializer : IJsonSerializer
{
    private static readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters =
        {
            new Int32JsonConverter(),
            new NumberToStringConverter()
        }
    };

    public T? Deserialize<T>(Stream content) where T : Base
    {
        return JsonSerializer.Deserialize<T>(content, serializerOptions);
    }
}

/// <summary>
///     Tiktok/Soundcloud returns width/height with percents, so I just extract numbers.
/// </summary>
internal class Int32JsonConverter : JsonConverter<int>
{
    public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.String)
        {
            string? stringValue = reader.GetString();
            if (stringValue != null)
            {
                if (stringValue.EndsWith('%'))
                {
                    stringValue = stringValue.Replace("%", "");
                }

                if (int.TryParse(stringValue, out int value))
                {
                    return value;
                }
            }
        }
        else if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetInt32(out int intValue))
            {
                return intValue;
            }

            if (reader.TryGetDouble(out double doubleValue))
            {
                return (int)doubleValue;
            }

            if (reader.TryGetDecimal(out decimal decimalValue))
            {
                return (int)decimalValue;
            }
        }

        return 0;
    }

    public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(value);
    }
}

internal class NumberToStringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Number)
        {
            if (reader.TryGetDecimal(out decimal single))
            {
                return single.ToString(CultureInfo.InvariantCulture);
            }

            if (reader.TryGetDouble(out double doubleNumber))
            {
                return doubleNumber.ToString(CultureInfo.InvariantCulture);
            }

            if (reader.TryGetInt32(out int number))
            {
                return number.ToString(CultureInfo.InvariantCulture);
            }
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            return reader.GetString();
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}