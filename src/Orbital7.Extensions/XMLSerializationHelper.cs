using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace Orbital7.Extensions
{
    public static partial class XMLSerializationHelper
    {
        public static T CloneObject<T>(T objectToClone)
        {
            return LoadFromXML<T>(SerializeToXML(objectToClone));
        }

        public static T LoadFromXML<T>(string xml)
        {
            return (T)LoadFromXML(typeof(T), xml);
        }

        public static object LoadFromXML(Type type, string xml)
        {
            object deserializedObject = null;

            using (var sr = new StringReader(xml.Trim()))
            {
                deserializedObject = LoadFromTextReader(type, sr);
            }

            return deserializedObject;
        }

        public static string SerializeToXML(object objectToSerialize)
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

        public static object LoadFromTextReader(Type type, TextReader textReader)
        {
            XmlSerializer xserDocumentSerializer = new XmlSerializer(type);
            return xserDocumentSerializer.Deserialize(XmlReader.Create(textReader, new XmlReaderSettings() { CheckCharacters = false }));
        }

        public static void SerializeToTextWriter(object objectToSerialize, TextWriter textWriter)
        {
            Type type = objectToSerialize.GetType();
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            xmlSerializer.Serialize(XmlWriter.Create(textWriter, new XmlWriterSettings() { CheckCharacters = false }), objectToSerialize);
        }
    }
}
