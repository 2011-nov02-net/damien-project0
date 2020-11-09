using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ArkhManufacturing.UserInterface.Converters
{
    public class DictionaryInt64Converter : JsonConverter<Dictionary<long, int>>
    {
        // TODO: Add comment here
        public override Dictionary<long, int> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            if (reader.TokenType != JsonTokenType.StartObject) {
                throw new JsonException();
            }

            var value = new Dictionary<long, int>();

            while (reader.Read()) {

                if (reader.TokenType == JsonTokenType.EndObject) {
                    return value;
                }

                string keyString = reader.GetString();

                if (!long.TryParse(keyString, out long keyAsInt64)) {
                    throw new JsonException($"Unable to convert '{keyString}' to long.");
                }

                reader.Read();

                string itemValue = reader.GetString();
                if (!int.TryParse(itemValue, out int valueAsInt32)) {
                    throw new JsonException($"Unable to convert '{keyString}' to int.");
                }

                value[keyAsInt64] = valueAsInt32;
            }

            throw new JsonException();
        }

        // TODO: Add comment here
        public override void Write(Utf8JsonWriter writer, Dictionary<long, int> value, JsonSerializerOptions options) {
            writer.WriteStartObject();

            foreach (var kv in value) {
                writer.WriteString(kv.Key.ToString(), kv.Value.ToString());
            }

            writer.WriteEndObject();
        }
    }
}
