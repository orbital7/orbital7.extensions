using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Orbital7.Extensions.Windows
{
    public static partial class XMLHelper
    {
        public static XmlAttribute AddAttribute(XmlNode nodeRoot, string attributeName, string attributeValue)
        {
            XmlAttribute attribute = nodeRoot.OwnerDocument.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            nodeRoot.Attributes.SetNamedItem(attribute);

            return attribute;
        }

        public static string GetAttributeValue(XmlNode nodeRoot, string xpath, string attributeName)
        {
            XmlNode target = nodeRoot.SelectSingleNode(xpath);
            return GetAttributeValue(target, attributeName);
        }

        public static string GetAttributeValue(XmlNode nodeRoot, string attributeName)
        {
            string value = String.Empty;

            if ((nodeRoot != null) && (nodeRoot.Attributes != null))
            {
                XmlAttribute attr = nodeRoot.Attributes[attributeName];
                if (attr != null)
                {
                    value = attr.Value;
                }
            }

            return value;
        }

        public static string GetNodeValue(XmlNode nodeRoot, string xpath)
        {
            string value = String.Empty;

            if (nodeRoot != null)
            {
                XmlNode target = nodeRoot.SelectSingleNode(xpath);
                if (target != null)
                {
                    value = target.InnerText;
                }
            }

            return value;
        }

        public static double GetNodeDoubleValue(XmlNode nodeRoot, string xpath)
        {
            string value = GetNodeValue(nodeRoot, xpath);
            if (!String.IsNullOrEmpty(value))
                return Convert.ToDouble(value);
            else
                return 0;
        }

        public static int GetNodeIntValue(XmlNode nodeRoot, string xpath)
        {
            string value = GetNodeValue(nodeRoot, xpath);
            if (!String.IsNullOrEmpty(value))
                return Convert.ToInt32(value);
            else
                return 0;
        }

        public static void WriteCDATANode(XmlTextWriter writer, string nodeName, string value)
        {
            WriteStringNode(writer, nodeName, value, true);
        }

        public static void WriteBooleanNode(XmlTextWriter writer, string nodeName, Boolean value)
        {
            writer.WriteStartElement(nodeName);
            writer.WriteValue(value);
            writer.WriteEndElement();
        }

        public static void WriteStringNode(XmlTextWriter writer, string nodeName, string value)
        {
            WriteStringNode(writer, nodeName, value, false);
        }

        public static void WriteStringNode(XmlTextWriter writer, string nodeName, string value, bool useCDATA)
        {
            if (value != null)
            {
                writer.WriteStartElement(nodeName);

                if (useCDATA)
                    writer.WriteCData(value);
                else
                    writer.WriteString(value);

                writer.WriteEndElement();
            }
        }

        public static void WriteStringListNode(XmlTextWriter writer, string nodeName, List<string> list)
        {
            if (list != null)
            {
                writer.WriteStartElement(nodeName);

                foreach (string value in list)
                    WriteStringNode(writer, "string", value);

                writer.WriteEndElement();
            }
        }

        public static XmlTextWriter CreateXmlTextWriter(string filePath, string nodeName)
        {
            XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8);
            writer.Formatting = Formatting.Indented;

            // Write the processing instructions.
            writer.WriteProcessingInstruction("xml", "version=\"1.0\" encoding=\"utf-8\"");

            // Add the root node.
            writer.WriteStartElement(nodeName);

            // Add the XML-Schema attributes.
            writer.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");

            return writer;
        }

        public static string ReadStringValue(XmlTextReader reader)
        {
            string value = String.Empty;

            if (!reader.IsEmptyElement)
            {
                reader.Read();
                if ((reader.NodeType == XmlNodeType.Text) || (reader.NodeType == XmlNodeType.CDATA))
                {
                    value = reader.Value;
                }
            }

            return value;
        }

        public static bool ReadBooleanValue(XmlTextReader reader)
        {
            return Convert.ToBoolean(ReadStringValue(reader));
        }

        public static short ReadShortValue(XmlTextReader reader)
        {
            return Convert.ToInt16(ReadStringValue(reader));
        }

        public static int ReadIntValue(XmlTextReader reader)
        {
            return Convert.ToInt32(ReadStringValue(reader));
        }

        public static double ReadDoubleValue(XmlTextReader reader)
        {
            return Convert.ToDouble(ReadStringValue(reader));
        }
    }
}
