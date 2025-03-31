using System.Xml;

namespace Orbital7.Extensions;

public static class XmlExtensions
{
    public static XmlAttribute AddAttribute(this XmlNode nodeRoot, string attributeName, string attributeValue)
    {
        XmlAttribute attribute = nodeRoot.OwnerDocument.CreateAttribute(attributeName);
        attribute.Value = attributeValue;
        nodeRoot.Attributes.SetNamedItem(attribute);

        return attribute;
    }

    public static string GetAttributeValue(this XmlNode nodeRoot, string xpath, string attributeName)
    {
        XmlNode target = nodeRoot.SelectSingleNode(xpath);
        return GetAttributeValue(target, attributeName);
    }

    public static string GetAttributeValue(this XmlNode nodeRoot, string attributeName)
    {
        string value = string.Empty;

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

    public static string GetNodeValue(this XmlNode nodeRoot, string xpath)
    {
        string value = string.Empty;

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

    public static double GetNodeDoubleValue(this XmlNode nodeRoot, string xpath)
    {
        string value = GetNodeValue(nodeRoot, xpath);
        if (!string.IsNullOrEmpty(value))
            return Convert.ToDouble(value);
        else
            return 0;
    }

    public static decimal GetNodeDecimalValue(this XmlNode nodeRoot, string xpath)
    {
        string value = GetNodeValue(nodeRoot, xpath);
        if (!string.IsNullOrEmpty(value))
            return Convert.ToDecimal(value);
        else
            return 0;
    }

    public static int GetNodeIntValue(this XmlNode nodeRoot, string xpath)
    {
        string value = GetNodeValue(nodeRoot, xpath);
        if (!string.IsNullOrEmpty(value))
            return Convert.ToInt32(value);
        else
            return 0;
    }

    public static bool GetNodeBoolValue(this XmlNode nodeRoot, string xpath)
    {
        string value = GetNodeValue(nodeRoot, xpath);
        if (!string.IsNullOrEmpty(value))
            return Convert.ToBoolean(value);
        else
            return false;
    }

    public static DateTime? GetNodeDateTimeValue(this XmlNode nodeRoot, string xpath)
    {
        string value = GetNodeValue(nodeRoot, xpath);
        if (!string.IsNullOrEmpty(value))
            return Convert.ToDateTime(value);
        else
            return null;
    }
}
