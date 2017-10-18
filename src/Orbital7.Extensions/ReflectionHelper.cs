using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Orbital7.Extensions
{
    public static class ReflectionHelper
    {
        public static string GetExecutingAssemblyFolderPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }

        public static List<T> CreateExternalInstances<T>()
        {
            return typeof(T).GetExternalTypes(GetExecutingAssemblyFolderPath()).CreateInstances<T>();
        }

        public static List<T> CreateExternalInstances<T>(string assembliesFolderPath)
        {
            return typeof(T).GetExternalTypes(assembliesFolderPath).CreateInstances<T>();
        }

        public static T CreateInstance<T>(string assemblyNameQualifiedTypeName, object[] parameters = null)
        {
            return CreateInstance<T>(Type.GetType(assemblyNameQualifiedTypeName), parameters);
        }

        public static T CreateInstance<T>(string assemblyName, string typeName, object[] parameters = null)
        {
            return CreateInstance<T>(Assembly.Load(assemblyName).GetType(typeName), parameters);
        }

        public static T CreateInstance<T>(object[] parameters = null)
        {
            return CreateInstance<T>(typeof(T), parameters);
        }

        private static T CreateInstance<T>(Type type, object[] parameters)
        {
            if (parameters != null)
                return type.CreateInstance<T>(parameters);
            else
                return type.CreateInstance<T>();
        }
    }
}
