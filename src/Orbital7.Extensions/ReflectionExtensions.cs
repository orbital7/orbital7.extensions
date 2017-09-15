using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace System
{
    public static class ReflectionExtensions
    {
        public static T GetAttribute<T>(this MemberInfo member, bool isRequired)
            where T : Attribute
        {
            var attribute = member.GetCustomAttributes(typeof(T), false).SingleOrDefault();

            if (attribute == null && isRequired)
            {
                throw new ArgumentException(
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "The {0} attribute must be defined on member {1}",
                        typeof(T).Name,
                        member.Name));
            }

            return (T)attribute;
        }

        public static List<Type> GetTypes(this Assembly assembly, Type type)
        {
            var types = new List<Type>();
            string targetInterface = type.ToString();

            foreach (var assemblyType in assembly.GetTypes())
            {
                if ((assemblyType.IsPublic) && (!assemblyType.IsAbstract))
                {
                    if (assemblyType.IsSubclassOf(type) || assemblyType.Equals(type) || (assemblyType.GetInterface(targetInterface) != null))
                        types.Add(assemblyType);
                }
            }

            return types;
        }
    }
}
