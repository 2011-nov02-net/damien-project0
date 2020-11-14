using System.IO;
using System.Text.Json;

using ArkhManufacturing.UserInterface.Converters;

namespace ArkhManufacturing.UserInterface.Serializers
{
    public class JsonDataSerializer<T> : IDataStorage<T>
    {
        private readonly string _filepath;

        // TODO: Add comment here
        public JsonDataSerializer(string filepath) => _filepath = filepath;

        // TODO: Add comment here
        public T Read() {
            string json = File.ReadAllText(_filepath);
            var jsonSerializerOptions = new JsonSerializerOptions();
            // jsonSerializerOptions.Converters.Add(new DictionaryInt64Converter());
            return JsonSerializer.Deserialize<T>(json, jsonSerializerOptions);
        }

        // TODO: Add comment here
        public void Commit(T data) {
            var jsonSerializerOptions = new JsonSerializerOptions { WriteIndented = true };
            jsonSerializerOptions.Converters.Add(new DictionaryInt64Converter());
            string json = JsonSerializer.Serialize<T>(data, jsonSerializerOptions);
            using var writer = new StreamWriter(_filepath);
            writer.Write(json);
        }
    }
}
