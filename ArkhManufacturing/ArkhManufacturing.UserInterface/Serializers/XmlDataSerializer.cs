using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace ArkhManufacturing.UserInterface.Serializers
{
    public class XmlDataSerializer<T> : IDataSerializer<T>
    {
        private readonly string _filepath;

        public XmlDataSerializer(string filepath) => _filepath = filepath;

        public T Read()
        {
            var dataContractSerializer = new DataContractSerializer(typeof(T));
            using var xmlReader = XmlReader.Create(_filepath);
            return (T)dataContractSerializer.ReadObject(xmlReader);
        }

        public void Write(T data)
        {
            var dataContractSerializer = new DataContractSerializer(typeof(T));
            using var xmlWriter = XmlWriter.Create(_filepath);
            dataContractSerializer.WriteObject(xmlWriter, data);
        }
    }
}
