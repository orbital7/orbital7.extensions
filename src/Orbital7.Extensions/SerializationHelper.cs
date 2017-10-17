using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Orbital7.Extensions
{
    public static partial class SerializationHelper
    {
        public static T CloneObject<T>(T objectToClone)
        {
            return LoadFromXml<T>(SerializeToXml(objectToClone));
        }

        public static T LoadFromXmlFile<T>(string filePath)
        {
            return (T)LoadFromXmlFile(typeof(T), filePath);
        }

        public static T LoadFromXml<T>(string xml)
        {
            return (T)LoadFromXml(typeof(T), xml);
        }

        public static object LoadFromXmlFile(Type type, string filePath)
        {
            return LoadFromXml(type, File.ReadAllText(filePath));
        }

        public static object LoadFromXml(Type type, string xml)
        {
            object deserializedObject = null;

            using (var sr = new StringReader(xml.Trim()))
            {
                deserializedObject = LoadFromTextReader(type, sr);
            }

            return deserializedObject;
        }

        public static void SerializeToXmlFile(object objectToSerialize, string filePath)
        {
            File.WriteAllText(filePath, SerializeToXml(objectToSerialize));
        }

        public static string SerializeToXml(object objectToSerialize)
        {
            return SerializeToStringWriter(objectToSerialize);
        }

        private static string SerializeToStringWriter(object objectToSerialize)
        {
            string value = String.Empty;

            using (StringWriter stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                SerializeToTextWriter(objectToSerialize, stringWriter);
                value = stringWriter.ToString();
            }

            return value;
        }

        private static object LoadFromTextReader(Type type, TextReader textReader)
        {
            XmlSerializer xserDocumentSerializer = new XmlSerializer(type);
            return xserDocumentSerializer.Deserialize(XmlReader.Create(textReader, new XmlReaderSettings() { CheckCharacters = false }));
        }

        private static void SerializeToTextWriter(object objectToSerialize, TextWriter textWriter)
        {
            Type type = objectToSerialize.GetType();
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            xmlSerializer.Serialize(XmlWriter.Create(textWriter, new XmlWriterSettings() { CheckCharacters = false }), objectToSerialize);
        }
    }
}
