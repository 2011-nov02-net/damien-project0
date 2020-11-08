using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace ArkhManufacturing.UserInterface.Serializers
{
    public class JsonDataSerializer<T> : IDataSerializer<T>
    {
        private readonly string _filepath;

        // TODO: Add comment here
        public JsonDataSerializer(string filepath) => _filepath = filepath;

        // TODO: Add comment here
        public T Read() {
            string json = File.ReadAllText(_filepath);
            return JsonSerializer.Deserialize<T>(json);
        }

        // TODO: Add comment here
        public void Write(T data) {
            string json = JsonSerializer.Serialize<T>(data, new JsonSerializerOptions { WriteIndented = true });
            using var writer = new StreamWriter(_filepath);
            writer.Write(json);
        }
    }
}
