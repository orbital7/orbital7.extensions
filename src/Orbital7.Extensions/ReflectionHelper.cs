﻿namespace System.Reflection;

public static class ReflectionHelper
{
    public static string GetExecutingAssemblyFolderPath()
    {
        string location = Assembly.GetExecutingAssembly().Location;
        UriBuilder uri = new UriBuilder(location);
        string path = Uri.UnescapeDataString(uri.Path);
        return Path.GetDirectoryName(path);
    }

    public static List<T> CreateExternalInstances<T>()
    {
        return typeof(T).GetExternalTypes(GetExecutingAssemblyFolderPath()).CreateInstances<T>();
    }

    public static List<T> CreateExternalInstances<T>(
        string assembliesFolderPath)
    {
        return typeof(T).GetExternalTypes(assembliesFolderPath).CreateInstances<T>();
    }

    public static T CreateInstance<T>(
        string assemblyQualifiedTypeName, 
        object[] parameters = null)
    {
        return CreateInstance<T>(Type.GetType(assemblyQualifiedTypeName), parameters);
    }

    public static T CreateInstance<T>(
        string assemblyName, 
        string typeName, 
        object[] parameters = null)
    {
        return CreateInstance<T>(Assembly.Load(assemblyName).GetType(typeName), parameters);
    }

    public static T CreateInstance<T>(
        object[] parameters = null)
    {
        return CreateInstance<T>(typeof(T), parameters);
    }

    public static List<T> CreateInstancesWithAttribute<T, TAttribute>()
        where TAttribute : Attribute
    {
        var types = Assembly
            .GetExecutingAssembly()
            .GetTypesWithAttribute<T, TAttribute>();

        return types
            .Select(x => x.CreateInstance<T>())
            .ToList();
    }

    private static T CreateInstance<T>(
        Type type, 
        object[] parameters)
    {
        if (parameters != null)
            return type.CreateInstance<T>(parameters);
        else
            return type.CreateInstance<T>();
    }
}
