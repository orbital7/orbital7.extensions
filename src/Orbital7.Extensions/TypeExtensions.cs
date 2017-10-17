using System;
using System.Collections.Generic;
using System.Text;

namespace Orbital7.Extensions
{
    public static class TypeExtensions
    {
        public static T CreateInstance<T>(this Type type)
        {
            return (T)Activator.CreateInstance(type);
        }

        public static T CreateInstance<T>(this Type type, object[] parameters)
        {
            return (T)Activator.CreateInstance(type, parameters);
        }

        public static List<T> GetTypeInstances<T>(this Type type, string folderPath)
        {
            var instances = new List<T>();

            var types = type.GetTypes(folderPath);

            foreach (Type typeItem in types)
            {
                try
                {
                    T instance = CreateInstance<T>(typeItem);
                    instances.Add(instance);
                }
                catch { }
            }

            return instances;
        }
    }
}
