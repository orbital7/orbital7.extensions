using System.Xml;
using System.Xml.Serialization;

namespace Orbital7.Extensions;

public static class XmlSerializationHelper
{

    public static string? StripInvalidXMLCharacters(
        string? value)
    {
        if (value != null)
        {
            StringBuilder textOut = new StringBuilder(); // Used to hold the output.   
            char current; // Used to reference the current character.   

            if (value == null || value == string.Empty) return string.Empty; // vacancy test.   
            for (int i = 0; i < value.Length; i++)
            {
                current = value[i];

                if ((current == 0x9 || current == 0xA || current == 0xD) ||
                    ((current >= 0x20) && (current <= 0xD7FF)) ||
                    ((current >= 0xE000) && (current <= 0xFFFD)))// ||   
                //((current >= 0x10000) && (current <= 0x10FFFF)))   
                {
                    textOut.Append(current);
                }
            }

            return textOut.ToString();
        }

        return null;
    }

    public static T? CloneObject<T>(
        T? objectToClone)
    {
        return DeserializeFromXml<T>(SerializeToXml(objectToClone));
    }

    public static T? DeserializeFromXml<T>(
        string? xml)
    {
        if (xml != null)
        {
            using (var reader = new StringReader(xml))
            {
                return (T?)DeserializeFromTextReader(typeof(T), reader);
            }
        }

        return default;
    }

    public static T? DeserializeFromXmlFile<T>(
        string filePath)
    {
        using (var reader = new StreamReader(filePath, false))
        {
            return (T?)DeserializeFromTextReader(typeof(T), reader);
        }
    }
    
    public static object? DeserializeFromXml(
        Type type, 
        string xml)
    {
        using (var reader = new StringReader(xml))
        {
            return DeserializeFromTextReader(type, reader);
        }
    }

    public static object? DeserializeFromXmlFile(
        Type type, 
        string filePath)
    {
        using (var reader = new StreamReader(filePath, false))
        {
            return DeserializeFromTextReader(type, reader);
        }
    }
    
    public static void SerializeToXmlFile(
        object objectToSerialize, 
        string filePath)
    {
        using (TextWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
        {
            SerializeToTextWriter(objectToSerialize, writer);
        }
    }

    public static string? SerializeToXml(
        object? objectToSerialize)
    {
        if (objectToSerialize != null)
        {
            using (StringWriter stringWriter = new StringWriterWithEncoding(Encoding.UTF8))
            {
                SerializeToTextWriter(objectToSerialize, stringWriter);
                return stringWriter.ToString();
            }
        }

        return null;
    }

    private static object? DeserializeFromTextReader(
        Type type, 
        TextReader textReader)
    {
        var xmlSerializer = new XmlSerializer(type);
        return xmlSerializer.Deserialize(XmlReader.Create(textReader, new XmlReaderSettings() { CheckCharacters = false }));
    }

    private static void SerializeToTextWriter(
        object objectToSerialize, 
        TextWriter textWriter)
    {
        var xmlWriter = XmlWriter.Create(textWriter, new XmlWriterSettings() { CheckCharacters = false });
        var xmlSerializer = new XmlSerializer(objectToSerialize.GetType());
        xmlSerializer.Serialize(xmlWriter, objectToSerialize);
        xmlWriter.Flush();
        xmlWriter.Close();
    }
}
