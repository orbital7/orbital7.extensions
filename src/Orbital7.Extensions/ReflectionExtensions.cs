using Orbital7.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System
{
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
                    if (Path.GetFileName(filePath).ToLower().StartsWith("msvc"))
                        continue;

                    var exType = GetExeType(filePath);
                    if (exType != EXEType.AnyCPU && exType != EXEType.Managed32 && exType != EXEType.Managed64)
                        continue;
                    if ((exType == EXEType.Managed32 && Environment.Is64BitProcess) || (exType == EXEType.Managed64 && !Environment.Is64BitProcess))
                        continue;

                    var assembly = Assembly.LoadFrom(filePath);
                    types.AddRange(assembly.GetTypes(baseType));
                }
                catch { }
            }

            return types;
        }

        private enum EXEType
        {
            Unknown,
            Native32,
            Managed32,
            Native64,
            Managed64,
            AnyCPU
        }

        private static EXEType GetExeType(string file)
        {
            // Check for managed code
            int bitSize = 0;
            string assemblyName = null;
            try
            {
                AssemblyName tname = AssemblyName.GetAssemblyName(file);
                ProcessorArchitecture arch = tname.ProcessorArchitecture;
                if (arch == ProcessorArchitecture.Amd64 || arch == ProcessorArchitecture.IA64)
                    bitSize = 64;
                else if (arch == ProcessorArchitecture.X86)
                    bitSize = 32;
                else if (arch == ProcessorArchitecture.MSIL)
                    bitSize = 3264;
                assemblyName = tname.FullName;
            }
            catch (Exception ex)
            {
                Exception ex2 = ex.InnerException;
            }

            // Get bit size from the PE header if it isn't managed
            if (bitSize == 0)
            {
                try
                {
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (BinaryReader br = new BinaryReader(fs))
                        {
                            UInt16 mz = br.ReadUInt16();
                            if (mz == 0x5a4d) // check if it's a valid image ("MZ")
                            {
                                fs.Position = 60; // this location contains the offset for the PE header
                                UInt32 peoffset = br.ReadUInt32();

                                fs.Position = peoffset + 4; // contains the architecture
                                UInt16 machine = br.ReadUInt16();

                                if (machine == 0x8664) // IMAGE_FILE_MACHINE_AMD64
                                    bitSize = 64;
                                else if (machine == 0x014c) // IMAGE_FILE_MACHINE_I386
                                    bitSize = 32;
                                else if (machine == 0x0200) // IMAGE_FILE_MACHINE_IA64
                                    bitSize = 64;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Exception ex2 = ex.InnerException;
                }
            }

            switch (bitSize)
            {
                case 32:
                    if (string.IsNullOrEmpty(assemblyName) == false)
                        return EXEType.Managed32;
                    return EXEType.Native32;
                case 64:
                    if (string.IsNullOrEmpty(assemblyName) == false)
                        return EXEType.Managed64;
                    return EXEType.Native64;
                case 3264:
                    return EXEType.AnyCPU;
            }

            return EXEType.Unknown;
        }
    }
}
