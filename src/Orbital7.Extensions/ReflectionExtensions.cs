using System.Linq.Expressions;
using System.Reflection;

namespace System;

public static class ReflectionExtensions
{
    public static MemberInfo GetPropertyInformation(this Expression propertyExpression)
    {
        MemberExpression memberExpr = propertyExpression as MemberExpression;
        if (memberExpr == null)
        {
            UnaryExpression unaryExpr = propertyExpression as UnaryExpression;
            if (unaryExpr != null && unaryExpr.NodeType == ExpressionType.Convert)
            {
                memberExpr = unaryExpr.Operand as MemberExpression;
            }
        }

        if (memberExpr != null && memberExpr.Member.MemberType == MemberTypes.Property)
        {
            return memberExpr.Member;
        }

        return null;
    }

    public static List<Type> GetTypes<T>(this Assembly assembly)
    {
        return assembly.GetTypes(typeof(T));
    }

    public static List<Type> GetTypes(this Assembly assembly, Type baseType)
    {
        var types = new List<Type>();
        string targetInterface = baseType.ToString();

        foreach (var assemblyType in assembly.GetTypes())
        {
            if ((assemblyType.IsPublic) && (!assemblyType.IsAbstract))
            {
                if (assemblyType.IsSubclassOf(baseType) || assemblyType.Equals(baseType) || (assemblyType.GetInterface(targetInterface) != null))
                    types.Add(assemblyType);
            }
        }

        return types;
    }

    public static T CreateInstance<T>(this Type type)
    {
        return (T)Activator.CreateInstance(type);
    }

    public static T CreateInstance<T>(this Type type, object[] parameters)
    {
        return (T)Activator.CreateInstance(type, parameters);
    }

    public static List<T> CreateInstances<T>(this List<Type> types)
    {
        var instances = new List<T>();

        foreach (Type typeItem in types)
        {
            try
            {
                T instance = typeItem.CreateInstance<T>();
                instances.Add(instance);
            }
            catch { }
        }

        return instances;
    }

    public static List<T> CreateInstances<T>(this Assembly assembly)
    {
        return assembly.GetTypes<T>().CreateInstances<T>();
    }

    public static List<T> CreateInstances<T>(this Assembly assembly, Type baseType)
    {
        return assembly.GetTypes(baseType).CreateInstances<T>();
    }

    public static List<Type> GetExternalTypes(this Type baseType)
    {
        return baseType.GetExternalTypes(ReflectionHelper.GetExecutingAssemblyFolderPath());
    }

    public static List<Type> GetExternalTypes(this Type baseType, string assembliesFolderPath)
    {
        List<Type> types = new List<Type>();

        foreach (string filePath in Directory.GetFiles(assembliesFolderPath, "*.dll"))
        {
            try
            {
                var assembly = Assembly.LoadFrom(filePath);
                types.AddRange(assembly.GetTypes(baseType));
            }
            catch { }
        }

        return types;
    }
}
