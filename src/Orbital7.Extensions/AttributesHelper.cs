using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Orbital7.Extensions
{
    public static class AttributesHelper
    {
        public static T GetAttribute<T>(Type objectType, string propertyName) where T : Attribute
        {
            Type attributeType = typeof(T);
            var propertyInfo = objectType.GetRuntimeProperty(propertyName);

            if (propertyInfo != null && propertyInfo.IsDefined(attributeType))
                return propertyInfo.GetCustomAttribute(attributeType) as T;

            return null;
        }

        public static object GetAttributeValue(Type objectType, string propertyName, Type attributeType, string attributePropertyName)
        {
            var propertyInfo = objectType.GetRuntimeProperty(propertyName);
            if (propertyInfo != null)
            {
                if (propertyInfo.IsDefined(attributeType))
                {
                    var attributeInstance = propertyInfo.GetCustomAttribute(attributeType);
                    if (attributeInstance != null)
                        foreach (PropertyInfo info in attributeType.GetRuntimeProperties())
                            if (info.CanRead && String.Compare(info.Name, attributePropertyName, StringComparison.CurrentCultureIgnoreCase) == 0)
                                return info.GetValue(attributeInstance, null);
                }
            }

            return null;
        }

        public static string GetPropertyDisplayName(Type objectType, string propertyName)
        {
            object displayName = GetAttributeValue(objectType, propertyName, typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), "Name");
            if (displayName != null)
                return displayName.ToString();
            else
                return propertyName;
        }

        public static string GetEnumDisplayName(object value, string nullValue = "")
        {
            if (value == null)
                return nullValue;

            var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
            if (fieldInfo == null)
                return nullValue;

            var descriptionAttributes = fieldInfo.GetCustomAttributes(
                typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false) as System.ComponentModel.DataAnnotations.DisplayAttribute[];

            if (descriptionAttributes == null) return value.ToString();
            return (descriptionAttributes.Length > 0) ? descriptionAttributes[0].Name : value.ToString();
        }

        public static IEnumerable<PropertyInfo> GetPropertiesWithAttribute(Type objectType, Type attributeType)
        {
            return objectType.GetRuntimeProperties().Where(prop => prop.IsDefined(attributeType));
        }

        public static IEnumerable<AttributePropertyPair<T>> GetPropertiesWithAttribute<T>(Type objectType) where T : Attribute
        {
            return from p in objectType.GetRuntimeProperties()
                   let attr = p.GetCustomAttributes(typeof(T), true).ToList()
                   where attr.Count == 1
                   select new AttributePropertyPair<T> { Property = p, Attribute = attr.First() as T };
        }

        //public static dynamic GetPropertiesWithAttribute<T>(Type objectType) where T : Attribute
        //{
        //    return from p in objectType.GetProperties()
        //           let attr = p.GetCustomAttributes(typeof(T), true)
        //           where attr.Length == 1
        //           select new { Property = p, Attribute = attr.First() as T };
        //}
    }

    public class AttributePropertyPair<T> where T : Attribute
    {
        public T Attribute { get; set; }
        public PropertyInfo Property { get; set; }
    }
}
