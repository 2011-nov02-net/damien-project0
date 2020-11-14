using System.Runtime.Serialization;
using System.Xml;

namespace ArkhManufacturing.UserInterface.Serializers
{
    public class XmlDataSerializer<T> : IDataStorage<T>
    {
        private readonly string _filepath;

        // TODO: Add comment here
        public XmlDataSerializer(string filepath) => _filepath = filepath;

        // TODO: Add comment here
        public T Read() {
            var dataContractSerializer = new DataContractSerializer(typeof(T));
            using var xmlReader = XmlReader.Create(_filepath);
            return (T)dataContractSerializer.ReadObject(xmlReader);
        }

        // TODO: Add comment here
        public void Commit(T data) {
            var dataContractSerializer = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings { Indent = true };
            using var xmlWriter = XmlWriter.Create(_filepath, settings);
            dataContractSerializer.WriteObject(xmlWriter, data);
        }
    }
}
