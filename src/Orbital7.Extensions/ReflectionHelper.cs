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

        public static T CreateInstance<T>(string assemblyNameQualifiedTypeName, object[] parameters = null)
        {
            var type = Type.GetType(assemblyNameQualifiedTypeName);

            if (parameters != null)
                return type.CreateInstance<T>(parameters);
            else
                return type.CreateInstance<T>();
        }

        public static T CreateInstance<T>(string assemblyName, string typeName, object[] parameters = null)
        {
            var type = Assembly.Load(assemblyName).GetType(typeName);

            if (parameters != null)
                return type.CreateInstance<T>(parameters);
            else
                return type.CreateInstance<T>();
        }
    }
}
