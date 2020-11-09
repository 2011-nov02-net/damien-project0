using System;
using System.Text.Json;
using System.Text.Json.Serialization;

using ArkhManufacturing.Library;

namespace ArkhManufacturing.UserInterface.Converters
{
    public class FranchiseConverter : JsonConverter<Franchise>
    {
        public override Franchise Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Franchise value, JsonSerializerOptions options) {
            throw new NotImplementedException();
        }
    }
}
