using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

using HeyRed.OEmbed.Abstractions;
using HeyRed.OEmbed.Models;

using Yoh.Text.Json.NamingPolicies;

namespace HeyRed.OEmbed.Defaults
{
    public class DefaultJsonSerializer : IJsonSerializer
    {
        private static readonly JsonSerializerOptions serializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicies.SnakeLowerCase,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters = { new Int32JsonConverter() }
        };

        public T? Deserialize<T>(Stream content) where T : Base => JsonSerializer.Deserialize<T>(content, serializerOptions);
    }

    /// <summary>
    /// Tiktok returns width/height with percents, so I just extract numbers.
    /// </summary>
    internal class Int32JsonConverter : JsonConverter<int>
    {
        public override int Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.String)
            {
                string? stringValue = reader.GetString();
                if (stringValue != null && stringValue.EndsWith('%'))
                {
                    if (int.TryParse(stringValue.Replace("%", ""), out int value))
                    {
                        return value;
                    }
                }
            }
            else if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32();
            }

            return 0;
        }

        public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(value);
        }
    }
}