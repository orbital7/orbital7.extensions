using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Orbital7.Extensions
{
    public static class SerializationHelper
    {
        public static T CloneObject<T>(T objectToClone)
        {
            return DeserializeFromXml<T>(SerializeToXml(objectToClone));
        }

        public static T DeserializeFromXml<T>(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                return (T)DeserializeFromTextReader(typeof(T), reader);
            }
        }

        public static T DeserializeFromXmlFile<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath, false))
            {
                return (T)DeserializeFromTextReader(typeof(T), reader);
            }
        }
        
        public static object DeserializeFromXml(Type type, string xml)
        {
            using (var reader = new StringReader(xml))
            {
                return DeserializeFromTextReader(type, reader);
            }
        }

        public static object DeserializeFromXmlFile(Type type, string filePath)
        {
            using (var reader = new StreamReader(filePath, false))
            {
                return DeserializeFromTextReader(type, reader);
            }
        }
        
        public static void SerializeToXmlFile(object objectToSerialize, string filePath)
        {
            using (TextWriter writer = new StreamWriter(filePath, false))
            {
                SerializeToTextWriter(objectToSerialize, writer);
            }
        }

        public static string SerializeToXml(object objectToSerialize)
        {
            using (StringWriter stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                SerializeToTextWriter(objectToSerialize, stringWriter);
                return stringWriter.ToString();
            }
        }

        private static object DeserializeFromTextReader(Type type, TextReader textReader)
        {
            var xmlSerializer = new XmlSerializer(type);
            return xmlSerializer.Deserialize(XmlReader.Create(textReader, new XmlReaderSettings() { CheckCharacters = false }));
        }

        private static void SerializeToTextWriter(object objectToSerialize, TextWriter textWriter)
        {
            var xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings() { CheckCharacters = false });
            var xmlSerializer = new XmlSerializer(objectToSerialize.GetType());
            xmlSerializer.Serialize(xmlWriter, objectToSerialize);
            xmlWriter.Flush();
            xmlWriter.Close();
        }
    }
}
